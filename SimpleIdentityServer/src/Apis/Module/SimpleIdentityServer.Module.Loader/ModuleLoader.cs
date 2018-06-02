using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Client;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Loader.Exceptions;
using SimpleIdentityServer.Module.Loader.Nuget;
using SimpleIdentityServer.Module.Loader.Nuget.DTOs.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SimpleIdentityServer.Module.Loader
{
    public interface IModuleLoader
    {
        void Initialize();
        Task RestorePackages();
        void LoadModules();
        void ConfigureServices(IServiceCollection services, IMvcBuilder mvcBuilder, IHostingEnvironment env);
        void Configure(IRouteBuilder routes);
        void Configure(IApplicationBuilder app);
        void CheckConfigurationFile();
        event EventHandler Initialized;
        event EventHandler<IntEventArgs> PackageRestored;
        event EventHandler ModulesLoaded;
        event EventHandler<StrEventArgs> ModuleInstalled;
        event EventHandler<StrEventArgs> ModuleCannotBeInstalled;
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

    internal sealed class ModuleLoader : IModuleLoader
    {

        private ConcurrentBag<string> _restoredPackages;
        private const string ENV_NAME = "SID_MODULE";
        private readonly INugetClient _nugetClient;
        private readonly IModuleFeedClientFactory _moduleFeedClientFactory;
        private readonly ModuleLoaderOptions _options;
        private string _modulePath;
        private const string _configFile = "config.json";
        private const string _configTemplateFile = "config.template.config";
        private ICollection<LoadedModule> _modules;
        private bool _isInitialized = false;
        private bool _isPackagesRestored = false;
        private bool _isConfigTemplateRestored = false;
        private ProjectResponse _projectConfiguration;

        public ModuleLoader(INugetClient nugetClient, IModuleFeedClientFactory moduleFeedClientFactory, ModuleLoaderOptions options)
        {
            if (nugetClient == null)
            {
                throw new ArgumentNullException(nameof(nugetClient));
            }

            if (moduleFeedClientFactory == null)
            {
                throw new ArgumentNullException(nameof(moduleFeedClientFactory));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            _nugetClient = nugetClient;
            _moduleFeedClientFactory = moduleFeedClientFactory;
            _options = options;
        }

        public event EventHandler Initialized;
        public event EventHandler<IntEventArgs> PackageRestored;
        public event EventHandler ModulesLoaded;
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

            _modulePath = Environment.GetEnvironmentVariable(ENV_NAME);
            if (string.IsNullOrWhiteSpace(_modulePath))
            {
                throw new ModuleLoaderConfigurationException($"The {ENV_NAME} environment variable must be set");
            }

            if (!Directory.Exists(_modulePath))
            {
                throw new ModuleLoaderConfigurationException($"The directory specified in the {ENV_NAME} environment variable doesn't exist");
            }

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
            if (_options.NugetSources == null || !_options.NugetSources.Any())
            {
                throw new ModuleLoaderConfigurationException("At least one nuget sources must be specified");
            }

            if(string.IsNullOrWhiteSpace(_options.ProjectName))
            {
                throw new ModuleLoaderConfigurationException("The project name must be specified");
            }

            if (_options.ModuleFeedUri == null)
            {
                throw new ModuleLoaderConfigurationException("The ModuleFeedUri parameter must be specified");
            }

            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _configFile);
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
            var errorMessages = new List<string>();
            var configTemplate = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), _configTemplateFile)));
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
        /// Restore the packages.
        /// </summary>
        /// <returns></returns>
        public async Task RestorePackages()
        {
            if (!_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is not initialized");
            }

            _restoredPackages = new ConcurrentBag<string>();
            await RestoreTemplateConfigurationFile();
            CheckConfigurationFile();
            var watch = Stopwatch.StartNew();
            foreach (var unit in _projectConfiguration.Units)
            {
                if (unit.Packages == null || !unit.Packages.Any())
                {
                    continue;
                }

                foreach(var package in unit.Packages)
                {
                    await RestorePackages(package.Library, package.Version);
                }
            }

            watch.Stop();
            Trace.WriteLine($"Finish to restore the packages in {watch.ElapsedMilliseconds} ms");
            _isPackagesRestored = true;
            if (PackageRestored != null)
            {
                PackageRestored(this, new IntEventArgs(watch.ElapsedMilliseconds));
            }
        }

        /// <summary>
        /// Load the modules.
        /// </summary>
        public void LoadModules()
        {
            if (!_isInitialized)
            {
                throw new ModuleLoaderInternalException("the loader is not initialized");
            }

            if (!_isPackagesRestored)
            {
                throw new ModuleLoaderInternalException("the packages are not restored");
            }

            if (!_isConfigTemplateRestored)
            {
                throw new ModuleLoaderInternalException("the config template is not restored");
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
                    var packagePath = Path.Combine(_modulePath, $"{package.Library}.{package.Version}\\lib");
                    if (!Directory.Exists(packagePath))
                    {
                        throw new ModuleLoaderInternalException($"The module {package.Library}.{package.Version} cannot be loaded");
                    }

                    var supportedFrameworks = GetSupportedFrameworks();
                    var fkDirectories = Directory.GetDirectories(packagePath);
                    var filteredFkDirectories = fkDirectories.Where(fkdir => supportedFrameworks.Any(sfk =>
                    {
                        var dirInfo = new DirectoryInfo(fkdir);
                        return sfk == dirInfo.Name;
                    })).OrderByDescending(s => s);
                    var dllPath = string.Empty;
                    if (filteredFkDirectories != null && filteredFkDirectories.Any())
                    {
                        dllPath = Path.Combine(filteredFkDirectories.First(), $"{package.Library}.dll");
                    }

                    if (string.IsNullOrWhiteSpace(dllPath) || !File.Exists(dllPath))
                    {
                        throw new ModuleLoaderInternalException($"The module {package.Library}.{package.Version} cannot be loaded");
                    }
                    
                    var assm = Assembly.LoadFile(dllPath);
                    var modules = assm.GetExportedTypes().Where(t => typeof(IModule).IsAssignableFrom(t));
                    if (modules == null || !modules.Any() || modules.Count() != 1)
                    {
                        throw new ModuleLoaderInternalException($"The module {package.Library}.{package.Version} doesn't contain an implementation of IModule");
                    }

                    var instance = (IModule)Activator.CreateInstance(modules.First());
                    _modules.Add(new LoadedModule(instance, package));
                }
            }

            if (ModulesLoaded != null)
            {
                ModulesLoaded(this, EventArgs.Empty);
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
        /// Register the services.
        /// </summary>
        public void ConfigureServices(IServiceCollection services, IMvcBuilder mvcBuilder, IHostingEnvironment env)
        {
            foreach (var loadedModule in _modules)
            {
                loadedModule.Instance.ConfigureServices(services, mvcBuilder, env, loadedModule.Unit.Parameters);
            }
        }

        public void Configure(IRouteBuilder routes)
        {
            foreach (var loadedModule in _modules)
            {
                loadedModule.Instance.Configure(routes);
            }
        }

        public void Configure(IApplicationBuilder app)
        {
            foreach (var loadedModule in _modules)
            {
                loadedModule.Instance.Configure(app);
            }
        }

        #region Private methods

        /// <summary>
        /// If the configuration template file doesn't exist then download it from the Module Feed Api and add it into the folder.
        /// If the configuration template exists then exit the method.
        /// </summary>
        /// <returns></returns>
        private async Task RestoreTemplateConfigurationFile()
        {
            var templateConfigurationFile = Path.Combine(Directory.GetCurrentDirectory(), _configTemplateFile);
            if (File.Exists(templateConfigurationFile))
            {
                _isConfigTemplateRestored = true;
                return;
            }

            var version = await ResolveVersion();
            using (var contentStream = await _moduleFeedClientFactory.BuildModuleFeedClient().GetProjectClient().Download(_options.ModuleFeedUri.AbsoluteUri, _options.ProjectName, version))
            {
                using (var stream = new FileStream(templateConfigurationFile, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await contentStream.CopyToAsync(stream);
                }
            }

            _isConfigTemplateRestored = true;
        }

        /// <summary>
        /// If the project doesn't exist then throw an exception.
        /// If the version doesn't exist or the version is null or empty then returns the latest one.
        /// Returns the requested version.
        /// </summary>
        /// <returns></returns>
        private async Task<string> ResolveVersion()
        {
            var projects = await _moduleFeedClientFactory.BuildModuleFeedClient().GetProjectClient().Get(_options.ModuleFeedUri.AbsoluteUri, _options.ProjectName);
            if (projects == null || !projects.Any())
            {
                throw new ModuleLoaderInternalException($"The project {_options.ProjectName} doesn't exist");
            }

            var version = _options.Version;
            var versions = projects.Select(p => p.Version);
            if (string.IsNullOrWhiteSpace(version) || !versions.Contains(version))
            {
                version = versions.OrderByDescending(v => v).First();
            }

            return version;
        }

        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains(".resources"))
            {
                return null;
            }

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
            {
                return assembly;
            }

            var splittedValue = args.Name.Split(',');
            if (splittedValue.Count() < 2)
            {
                return null;
            }

            var packageName = splittedValue.First().Replace(" ", "");
            IEnumerable<string> splittedVersion = splittedValue.ElementAt(1).Replace(" ", "").Split('=');
            if (splittedVersion.Count() != 2)
            {
                return null;
            }

            splittedVersion = splittedVersion.ElementAt(1).Split('.');
            if (splittedVersion.Count() > 3)
            {
                splittedVersion = splittedVersion.Take(3);
            }

            var version = string.Join(".", splittedVersion);
            var subVersion = string.Join(".", splittedVersion.Take(2));
            var baseVersion = splittedVersion.ElementAt(0);
            var moduleDirectories = Directory.GetDirectories(_modulePath, $"{packageName}.{version}*");
            if (moduleDirectories == null || !moduleDirectories.Any())
            {
                moduleDirectories = Directory.GetDirectories(_modulePath, $"{packageName}.{subVersion}*");
                if (moduleDirectories == null || !moduleDirectories.Any())
                {
                    moduleDirectories = Directory.GetDirectories(_modulePath, $"{packageName}.{baseVersion}*");
                    if (moduleDirectories == null || !moduleDirectories.Any())
                    {
                        moduleDirectories = Directory.GetDirectories(_modulePath, $"{packageName}.*");
                        if (moduleDirectories == null || !moduleDirectories.Any())
                        {
                            return null;
                        }
                    }
                }
            }            

            var libPath = Path.Combine(moduleDirectories.First(), "lib");
            if (!Directory.Exists(libPath))
            {
                return null;
            }

            var fkDirectories = Directory.GetDirectories(libPath);
            var dllPath = string.Empty;
            if (fkDirectories.Count() == 1)
            {
                dllPath  = Path.Combine(fkDirectories.First(), $"{packageName}.dll");
            }
            else
            {
                var supportedFrameworks = GetSupportedFrameworks();
                var filteredFkDirectories = fkDirectories.Where(fkdir => supportedFrameworks.Any(sfk =>
                {
                    var dirInfo = new DirectoryInfo(fkdir);
                    return sfk == dirInfo.Name;
                })).OrderByDescending(s => s);
                if (filteredFkDirectories != null && filteredFkDirectories.Any())
                {
                    dllPath = Path.Combine(filteredFkDirectories.First(), $"{packageName}.dll");
                }
            }

            if (!string.IsNullOrWhiteSpace(dllPath) && File.Exists(dllPath))
            {
                var assm = Assembly.LoadFrom(dllPath);
                return assm;
            }

            return null;
        }

        private async Task RestorePackages(string packageName, string version)
        {
            var key = $"{packageName}_{version}";
            if (_restoredPackages.Contains(key))
            {
                return;
            }

            _restoredPackages.Add(key);
            if (string.IsNullOrWhiteSpace(packageName) || string.IsNullOrWhiteSpace(version))
            {
                return;
            }

            var pkgName = $"{packageName}.{version}";
            var packagePath = Path.Combine(_modulePath, pkgName);
            var nuSpecPath = Path.Combine(_modulePath, pkgName, packageName + ".nuspec");
            if (!Directory.Exists(packagePath))
            {
                await DownloadNugetPackage(packageName, version);
            }
            
            if (!File.Exists(nuSpecPath))
            {
                return;
            }

            var nugetDependencies = new List<NugetDependency>();
            var xmlDoc = new XmlDocument();
            using (var reader = XmlReader.Create(nuSpecPath))
            {
                xmlDoc.Load(reader);
            }

            List<NugetGroup> groups = null;
            using (var strReader = new StringReader(xmlDoc.OuterXml))
            {
                switch (xmlDoc.DocumentElement.NamespaceURI)
                {
                    case "http://schemas.microsoft.com/packaging/2012/06/nuspec.xsd":
                        var serializer2012 = new XmlSerializer(typeof(NugetSpecification2012));
                        var nugetSpecification2012 = (NugetSpecification2012)serializer2012.Deserialize(strReader);
                        groups = nugetSpecification2012.Metadata.Dependencies.Groups;
                        break;
                    case "http://schemas.microsoft.com/packaging/2013/05/nuspec.xsd":
                        var serializer2013 = new XmlSerializer(typeof(NugetSpecification2013));
                        var nugetSpecification2013 = (NugetSpecification2013)serializer2013.Deserialize(strReader);
                        groups = nugetSpecification2013.Metadata.Dependencies.Groups;
                        break;
                }
            }

            if (groups == null)
            {
                return;
            }

            foreach(var group in groups)
            {
                if (group.Dependencies == null)
                {
                    continue;
                }

                foreach(var dependency in group.Dependencies)
                {
                    if (!nugetDependencies.Any(nd => $"{nd.Id}.{nd.Version}" == $"{dependency.Id}.{dependency.Version}"))
                    {
                        nugetDependencies.Add(dependency);
                    }
                }
            }

            var operations = new List<Task>();
            foreach(var nugetDependency in nugetDependencies)
            {
                operations.Add(RestorePackages(nugetDependency.Id, nugetDependency.Version));
            }

            await Task.WhenAll(operations);
        }

        private async Task DownloadNugetPackage(string packageName, string version)
        {
            var pkgSubPath = $"{packageName}.{version}";
            var pkgFileSubPath = pkgSubPath + ".nupkg";
            var pkgPath = Path.Combine(_modulePath, pkgSubPath);
            var pkgFilePath = Path.Combine(_modulePath, pkgFileSubPath);
            foreach (var nugetSource in _options.NugetSources)
            {
                Uri uriResult;
                if (!Uri.TryCreate(nugetSource, UriKind.Absolute, out uriResult))
                {
                    continue;
                }

                if (Directory.Exists(nugetSource))
                {
                    var files = Directory.GetFiles(nugetSource, pkgFileSubPath);
                    if (files == null || !files.Any())
                    {
                        continue;
                    }

                    File.Copy(files.First(), pkgFilePath);
                }
                else
                {

                    var configuration = await _nugetClient.GetConfiguration(nugetSource);
                    if (configuration == null)
                    {
                        continue;
                    }

                    var pkgBaseAdr = configuration.Resources.FirstOrDefault(r => r.Type.Contains("PackageBaseAddress"));
                    if (pkgBaseAdr == null)
                    {
                        continue;
                    }

                    NugetFlatContainerResponse flatContainerResponse = null;
                    try
                    {
                        flatContainerResponse = await _nugetClient.GetNugetFlatContainer(pkgBaseAdr.Id, packageName);
                    }
                    catch (Exception)
                    {
                        continue;
                    }

                    if (flatContainerResponse == null || !flatContainerResponse.Versions.Contains(version))
                    {
                        continue;
                    }

                    var cannotDownload = false;
                    int i = 1;
                    do
                    {
                        try
                        {
                            using (var contentStream = await _nugetClient.DownloadNugetPackage(pkgBaseAdr.Id, packageName, version))
                            {
                                using (var stream = new FileStream(pkgFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                                {
                                    await contentStream.CopyToAsync(stream);
                                }
                            }

                            cannotDownload = false;
                        }
                        catch (Exception)
                        {
                            cannotDownload = true;
                            if (ModuleCannotBeInstalled != null)
                            {
                                ModuleCannotBeInstalled(this, new StrEventArgs(packageName));
                            }

                            Thread.Sleep(_options.NugetRetryAfterMs);
                        }
                        finally
                        {
                            i++;
                        }
                    } while (cannotDownload && i <= _options.NugetNbRetry);
                }

                ZipFile.ExtractToDirectory(pkgFilePath, pkgPath);
                File.Delete(pkgFilePath);
                Trace.WriteLine($"The package {packageName} is installed");
                if (ModuleInstalled != null)
                {
                    ModuleInstalled(this, new StrEventArgs(pkgSubPath));
                }

                return;
            }
        }

        private IEnumerable<string> GetSupportedFrameworks()
        {
#if NET461
            return new List<string>
            {
                "net461",
                "net46",
                "net45",
                "net40",
                "net35"
            };
#else
            return new List<string>
            {
                "netstandard2.0",
                "netstandard1.6",
                "netstandard1.0",
                "netcoreapp2.0"
            };
#endif
        }

        #endregion
    }
}
