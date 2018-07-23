using Moq;
using SimpleIdentityServer.Module.Feed.Client;
using SimpleIdentityServer.Module.Feed.Client.Projects;
using SimpleIdentityServer.Module.Loader.Exceptions;
using SimpleIdentityServer.Module.Loader.Factories;
using SimpleIdentityServer.Module.Loader.Nuget;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Loader.Tests
{
    public class ModuleLoaderFixture
    {
        private const string _templateJson = "{" +
              "\"id\": \"projectName_3.0.0-rc7\"," +
              "\"version\": \"3.0.0-rc7\"," +
              "\"name\": \"projectName\"," +
              "\"units\": [" +
                "{" +
                  "\"name\": \"unit1\"," +
                  "\"packages\": [" +
                    "{" +
                      "\"lib\": \"lib1\"," +
                      "\"version\": \"1\"," +
                      "\"category\": \"cat1\"" +
                    "}," +
                    "{" +
                      "\"lib\": \"lib2\"," +
                      "\"version\": \"2\"," +
                      "\"category\": \"cat2\"" +
                    "}" +
                  "]" +
                "}" +
              "]" +
            "}";
        private const string _configJson = "{" +
              "\"id\": \"projectName_3.0.0-rc8\"," +
              "\"version\": \"3.0.0-rc8\", " +
              "\"name\": \"projectName\", " +
              "\"units\": [" +
                "{" +
                            "\"name\": \"unit1\"" +
                "}" +
              "]" +
            "}";

        [Fact]
        public void WhenPassingNullParameterToConstructorThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(new NugetClient(new HttpClientFactory()), null, null));
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(new NugetClient(new HttpClientFactory()), new ModuleFeedClientFactory(), null));
        }

        #region Initialize

        [Fact]
        public void WhenInitializeModuleLoaderAndNoEnvironmentVariableThenExceptionIsThrown()
        {
            // ARRANGE
            Environment.SetEnvironmentVariable("SID_MODULE", null);
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions();
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal($"The SID_MODULE environment variable must be set", exception.Message);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndEnvironmentVariableIsNotValidThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions();
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);
            Environment.SetEnvironmentVariable("SID_MODULE", "invalid");

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal("The directory specified in the SID_MODULE environment variable doesn't exist", exception.Message);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndNugetSourcesIsNullThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = null
            };
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal("At least one nuget sources must be specified", exception.Message);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndProjectNameIsNullThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = new[] { "nuget" },
                ProjectName = null
            };
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal("The project name must be specified", exception.Message);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndModuleFeedUriIsNullThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = new[] { "nuget" },
                ProjectName = "projectName"
            };
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal("The ModuleFeedUri parameter must be specified", exception.Message);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndConfigurationFileDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            SetCurrentAssembly();
            RemoveFiles();
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = new[] { "nuget" },
                ProjectName = "projectName",
                ModuleFeedUri = new Uri("http://localhost/configuration")
            };
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<FileNotFoundException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
        }

        #endregion

        #region RestorePackages

        [Fact]
        public async Task WhenRestorePackagesAndModuleLoaderIsNotInitializedThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions();
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var ex = await Assert.ThrowsAsync<ModuleLoaderInternalException>(() => moduleLoader.RestoreUnits());

            // ASSERT
            Assert.NotNull(ex);
        }

        /*
        [Fact]
        public async Task WhenRestorePackagesAndNoProjectExistsThenExceptionIsThrown()
        {
            // ARRANGE
            SetCurrentAssembly();
            RemoveFiles();
            AddConfigFile();
            var nugetClientMock = new Mock<INugetClient>();
            var projectClientMock = new Mock<IProjectClient>();
            var moduleFeedClientMock = new Mock<IModuleFeedClient>();
            var moduleFeedClientFactoryMock = new Mock<IModuleFeedClientFactory>();
            projectClientMock.Setup(d => d.Download(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Stream>(new MemoryStream(Encoding.UTF8.GetBytes(_templateJson))));
            moduleFeedClientMock.Setup(m => m.GetProjectClient()).Returns(projectClientMock.Object);
            moduleFeedClientFactoryMock.Setup(m => m.BuildModuleFeedClient()).Returns(moduleFeedClientMock.Object);
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = new[] { "nuget" },
                ProjectName = "projectName",
                ModuleFeedUri = new Uri("http://localhost/configuration")
            };

            // ACT
            var moduleLoader = new ModuleLoader(nugetClientMock.Object, moduleFeedClientFactoryMock.Object, options);
            moduleLoader.Initialize();
            var exception = await Assert.ThrowsAsync<ModuleLoaderInternalException>(() => moduleLoader.RestoreUnits());

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal($"The project {options.ProjectName} doesn't exist", exception.Message);
        }
        */
        
        #endregion

        #region CheckConfigurationFile

        /*
        [Fact]
        public void WhenCheckConfigurationFileThenExceptionIsThrown()
        {
            // ARRANGE
            SetCurrentAssembly();
            RemoveFiles();
            AddTemplateFile();
            AddConfigFile();
            var nugetClientMock = new Mock<INugetClient>();
            var moduleFeedClientFactoryMock = new Mock<IModuleFeedClientFactory>();
            Environment.SetEnvironmentVariable("SID_MODULE", "C:\\");
            var options = new ModuleLoaderOptions
            {
                NugetSources = new[] { "nuget" },
                ProjectName = "projectName",
                ModuleFeedUri = new Uri("http://localhost/configuration")
            };

            // ACT
            var moduleLoader = new ModuleLoader(nugetClientMock.Object, moduleFeedClientFactoryMock.Object, options);
            moduleLoader.Initialize();
            var exception = Assert.Throws<ModuleLoaderAggregateConfigurationException>(() => moduleLoader.CheckConfigurationFile());

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal(4, exception.Messages.Count());
        }
        */

        #endregion

        private void RemoveFiles()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.json")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "config.json"));
            }
            
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.template.json")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "config.template.json"));
            }
        }

        private void AddTemplateFile()
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.template.json"), _templateJson);
        }

        private void AddConfigFile()
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.json"), _configJson);
        }

        private void SetCurrentAssembly()
        {
            var assembly = Assembly.GetCallingAssembly();
            AppDomainManager manager = new AppDomainManager();
            FieldInfo entryAssemblyfield = manager.GetType().GetField("m_entryAssembly", BindingFlags.Instance | BindingFlags.NonPublic);
            entryAssemblyfield.SetValue(manager, assembly);
            AppDomain domain = AppDomain.CurrentDomain;
            FieldInfo domainManagerField = domain.GetType().GetField("_domainManager", BindingFlags.Instance | BindingFlags.NonPublic);
            domainManagerField.SetValue(domain, manager);
        }
    }
}