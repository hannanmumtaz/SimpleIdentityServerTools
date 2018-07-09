using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleIdentityServer.Connectors.Common;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Loader.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SimpleIdentityServer.Module.Loader
{
    public interface IModuleLoader
    {
        void Initialize();
        void LoadUnits();
        void LoadConnectors();
        void LoadTwoFactors();
        void ConfigureUnitsServices(IServiceCollection services, IMvcBuilder mvcBuilder, IHostingEnvironment env);
        void ConfigureUnitsAuthorization(AuthorizationOptions authorizationOptions);
        void ConfigureConnectors(AuthenticationBuilder authBuilder);
        void ConfigureTwoFactors(IServiceCollection services);
        void ConfigureApplicationBuilder(IApplicationBuilder app);
        void ConfigureRouter(IRouteBuilder router);
        event EventHandler UnitsLoaded;
        event EventHandler ConnectorsLoaded;
        event EventHandler TwoFactorsLoaded;
    }

    public class StrEventArgs : EventArgs
    {
        public StrEventArgs(string s)
        {
            Value = s;
        }

        public string Value { get; private set; }
    }

    public class IntEventArgs : EventArgs
    {
        public IntEventArgs(long i)
        {
            Value = i;
        }

        public long Value { get; private set; }
    }

    public class LoadedModule
    {
        public LoadedModule(IModule instance, UnitPackageResponse unit)
        {
            Instance = instance;
            Unit = unit;
        }

        public IModule Instance { get; private set; }
        public UnitPackageResponse Unit { get; private set; }
    }

    public class LoadedConnector
    {
        public LoadedConnector(IConnector instance, ProjectConnectorResponse connector)
        {
            Instance = instance;
            Connector = connector;
        }

        public IConnector Instance { get; private set; }
        public ProjectConnectorResponse Connector { get; private set; }
    }

    public class LoadedTwoFactor
    {
        public LoadedTwoFactor(IModule instance, ProjectTwoFactorAuthenticator twoFactor)
        {
            Instance = instance;
            TwoFactor = twoFactor;
        }

        public IModule Instance { get; private set; }
        public ProjectTwoFactorAuthenticator TwoFactor { get; private set; }
    }

    internal sealed class ModuleLoader : IModuleLoader
    {
        private FileSystemWatcher _watcher;
        private ConcurrentBag<string> _restoredPackages;
        private readonly ModuleLoaderOptions _options;
        private const string _configFile = "config.json";
        private const string _configTemplateFile = "config.template.json";
        private ICollection<LoadedModule> _modules;
        private ICollection<LoadedConnector> _connectors;
        private ICollection<LoadedTwoFactor> _twoFactors;
        private bool _isInitialized = false;
        private DateTime _lastWriteTime;
        private ProjectResponse _projectConfiguration;

        public ModuleLoader(ModuleLoaderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _options = options;
        }

        public event EventHandler Initialized;
        public event EventHandler<IntEventArgs> ConnectorsRestored;
        public event EventHandler ConnectorsChanged;
        public event EventHandler UnitsLoaded;
        public event EventHandler TwoFactorsLoaded;
        public event EventHandler ConnectorsLoaded;
        public event EventHandler<StrEventArgs> ModuleInstalled;
        public event EventHandler<StrEventArgs> ModuleCannotBeInstalled;

        /// <summary>
        /// Initialize the module loader.
        /// </summary>
        public void Initialize()
        {
            if (_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is already initialized");
            }
            
            if(string.IsNullOrWhiteSpace(_options.ProjectName))
            {
                throw new ModuleLoaderConfigurationException("The project name must be specified");
            }

            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var configurationFilePath = Path.Combine(currentDirectory, _configFile);
            if (!File.Exists(configurationFilePath))
            {
                throw new FileNotFoundException(configurationFilePath);
            }

            var json = File.ReadAllText(configurationFilePath);
            _projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(json);
            if (_projectConfiguration == null)
            {
                throw new ModuleLoaderConfigurationException($"{configurationFilePath} is not a valid configuration file");
            }

            _isInitialized = true;
            CheckConfigurationFile();
            if (Initialized != null)
            {
                Initialized(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Check the configuration file structure.
        /// </summary>
        /// <returns></returns>
        public void CheckConfigurationFile()
        {
            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var errorMessages = new List<string>();
            var configTemplate = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(Path.Combine(currentDirectory, _configTemplateFile)));
            if (configTemplate.Id != _projectConfiguration.Id)
            {
                errorMessages.Add("The config identifier is not the same than the one in the configuration template file");
            }

            if (configTemplate.ProjectName != _projectConfiguration.ProjectName)
            {
                errorMessages.Add("The config projectName is not the same than the one in the configuration template file");
            }

            if (configTemplate.Version != _projectConfiguration.Version)
            {
                errorMessages.Add("The config version is not the same than the one in the configuration template file");
            }

            if (configTemplate.Units != null)
            {
                foreach (var unit in configTemplate.Units)
                {
                    var configUnit = _projectConfiguration.Units.FirstOrDefault(u => u.UnitName == unit.UnitName);
                    if (configUnit == null)
                    {
                        errorMessages.Add($"The unit {unit.UnitName} doesn't exist");
                        continue;
                    }

                    foreach (var groupedPackages in unit.Packages.GroupBy(p => p.CategoryName))
                    {
                        var configPackage = configUnit.Packages == null ? null : configUnit.Packages.FirstOrDefault(p => groupedPackages.Any(gp => gp.Library == p.Library &&
                             gp.Version == p.Version &&
                             gp.CategoryName == groupedPackages.Key));
                        if (configPackage == null)
                        {
                            errorMessages.Add($"One of the following package {string.Join(";", groupedPackages.Select(gp => gp.Library).ToArray())} must be installed under the unit {unit.UnitName}");
                        }
                    }
                }
            }

            if (errorMessages.Any())
            {
                throw new ModuleLoaderAggregateConfigurationException("invalid configuration", errorMessages);
            }
        }

        /// <summary>
        /// Load the units.
        /// </summary>
        public void LoadUnits()
        {
            if (!_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is not initialized");
            }

            _modules = new List<LoadedModule>();
            if (_projectConfiguration.Units == null || !_projectConfiguration.Units.Any())
            {
                return;
            }

            foreach(var unit in _projectConfiguration.Units)
            {
                if (unit.Packages == null || !unit.Packages.Any())
                {
                    continue;
                }

                foreach(var package in unit.Packages)
                {
                    var instance = LoadLibrary<IModule>(package.Library);
                    _modules.Add(new LoadedModule(instance, package));
                }
            }

            if (UnitsLoaded != null)
            {
                UnitsLoaded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Load the connectors.
        /// </summary>
        public void LoadConnectors()
        {
            if (!_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is not initialized");
            }

            _connectors = new List<LoadedConnector>();
            if (_projectConfiguration.Connectors == null || !_projectConfiguration.Connectors.Any())
            {
                return;
            }

            foreach(var connector in _projectConfiguration.Connectors)
            {
                if (string.IsNullOrWhiteSpace(connector.Library) || string.IsNullOrWhiteSpace(connector.Version))
                {
                    continue;
                }

                var instance = LoadLibrary<IConnector>(connector.Library);
                _connectors.Add(new LoadedConnector(instance, connector));
            }

            if (ConnectorsLoaded != null)
            {
                ConnectorsLoaded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Load the two factors.
        /// </summary>
        public void LoadTwoFactors()
        {
            if (!_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is not initialized");
            }

            _twoFactors = new List<LoadedTwoFactor>();
            if (_projectConfiguration.TwoFactors == null || !_projectConfiguration.TwoFactors.Any())
            {
                return;
            }

            foreach (var twoFactor in _projectConfiguration.TwoFactors)
            {
                if (string.IsNullOrWhiteSpace(twoFactor.Library) || string.IsNullOrWhiteSpace(twoFactor.Version))
                {
                    continue;
                }

                var instance = LoadLibrary<IModule>(twoFactor.Library);
                _twoFactors.Add(new LoadedTwoFactor(instance, twoFactor));
            }

            if (TwoFactorsLoaded != null)
            {
                TwoFactorsLoaded(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Configure the units services.
        /// </summary>
        public void ConfigureUnitsServices(IServiceCollection services, IMvcBuilder mvcBuilder, IHostingEnvironment env)
        {
            if (_modules != null)
            {
                foreach (var loadedModule in _modules)
                {
                    loadedModule.Instance.ConfigureServices(services, mvcBuilder, env, loadedModule.Unit.Parameters);
                }
            }
        }

        /// <summary>
        /// Configure the units authorization.
        /// </summary>
        /// <param name="authorizationOptions"></param>
        public void ConfigureUnitsAuthorization(AuthorizationOptions authorizationOptions)
        {
            if (_modules != null)
            {
                foreach (var loadedModule in _modules)
                {
                    loadedModule.Instance.ConfigureAuthorization(authorizationOptions, loadedModule.Unit.Parameters);
                }
            }
        }

        /// <summary>
        /// Configure the connectors
        /// </summary>
        /// <param name="authBuilder"></param>
        public void ConfigureConnectors(AuthenticationBuilder authBuilder)
        {
            if (authBuilder == null)
            {
                throw new ArgumentNullException(nameof(authBuilder));
            }

            if (_connectors == null)
            {
                return;
            }

            foreach(var connector in _connectors)
            {
                connector.Instance.Configure(authBuilder, connector.Connector.Parameters);
            }
        }

        /// <summary>
        /// Configure the two factors.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureTwoFactors(IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (_twoFactors == null)
            {
                return;
            }

            foreach(var twoFactor in _twoFactors)
            {
                twoFactor.Instance.ConfigureServices(services, null, null, twoFactor.TwoFactor.Parameters);
            }
        }

        /// <summary>
        /// Configure application builder.
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureApplicationBuilder(IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException(nameof(app));
            }

            foreach (var loadedModule in _modules)
            {
                loadedModule.Instance.Configure(app);
            }
        }

        /// <summary>
        /// Configure ASP.NET MVC routing.
        /// </summary>
        /// <param name="routes"></param>
        public void ConfigureRouter(IRouteBuilder router)
        {
            if (router == null)
            {
                throw new ArgumentNullException(nameof(router));
            }

            foreach (var loadedModule in _modules)
            {
                loadedModule.Instance.Configure(router);
            }
        }

        /// <summary>
        /// Returns the list of loaded modules.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LoadedModule> GetModules()
        {
            return _modules;
        }

        /// <summary>
        /// Returns the list of loaded connectors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LoadedConnector> GetConnectors()
        {
            return _connectors;
        }

        /// <summary>
        /// Returns the list of loaded two factors.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<LoadedTwoFactor> GetTwoFactors()
        {
            return _twoFactors;
        }

        /// <summary>
        /// Configure the module authentication
        /// </summary>
        /// <param name="localAuthBuilder"></param>
        /// <param name="externalAuthBuilder"></param>
        public void ConfigureModuleAuthentication(AuthenticationBuilder localAuthBuilder)
        {
            if (_modules != null)
            {
                foreach (var loadedModule in _modules)
                {
                    loadedModule.Instance.ConfigureAuthentication(localAuthBuilder, loadedModule.Unit.Parameters);
                }
            }
        }

        /// <summary>
        /// Configure the connectors authentication.
        /// </summary>
        /// <param name="localAuthBuilder"></param>
        public void ConfigureConnectorAuthentication(AuthenticationBuilder localAuthBuilder)
        {
            if (_connectors != null)
            {
                foreach (var connector in _connectors)
                {
                    connector.Instance.Configure(localAuthBuilder, connector.Connector.Parameters);
                }
            }
        }

        #region Private methods

        private T LoadLibrary<T>(string library)
        {
            // string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), $"{library}.dll");
            // var assm = Assembly.LoadFrom(path);
            var assm = Assembly.Load(library);
            if (assm == null)
            {
                throw new ModuleLoaderInternalException($"The module {library}cannot be loaded");
            }

            var modules = assm.GetExportedTypes().Where(t => typeof(T).IsAssignableFrom(t));
            if (modules == null || !modules.Any() || modules.Count() != 1)
            {
                throw new ModuleLoaderInternalException($"The module {library} doesn't contain an implementation of {typeof(T).GetType().Name}");
            }

            return (T)Activator.CreateInstance(modules.First());
        }

        #endregion
    }
}
