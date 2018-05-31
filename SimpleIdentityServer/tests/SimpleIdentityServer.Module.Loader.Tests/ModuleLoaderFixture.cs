using Moq;
using SimpleIdentityServer.Module.Feed.Client;
using SimpleIdentityServer.Module.Feed.Client.Projects;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Loader.Exceptions;
using SimpleIdentityServer.Module.Loader.Factories;
using SimpleIdentityServer.Module.Loader.Nuget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Module.Loader.Tests
{
    public class ModuleLoaderFixture
    {
        [Fact]
        public void WhenPassingNullParameterToConstructorThenExceptionIsThrown()
        {
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(null, null, null));
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(new NugetClient(new HttpClientFactory()), null, null));
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(new NugetClient(new HttpClientFactory()), new ModuleFeedClientFactory(), null));
        }

        #region Initialize

        [Fact]
        public void WhenInitializeModuleLoaderAndModulePathDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions
            {
                ModulePath = "bad_path"
            };
            var moduleLoader = new ModuleLoader(nugetClient, moduleFeedClientFactory, options);

            // ACT
            var exception = Assert.Throws<DirectoryNotFoundException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
        }

        [Fact]
        public void WhenInitializeModuleLoaderAndNugetSourcesIsNullThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions
            {
                ModulePath = Directory.GetCurrentDirectory(),
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
            var options = new ModuleLoaderOptions
            {
                ModulePath = Directory.GetCurrentDirectory(),
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
            var options = new ModuleLoaderOptions
            {
                ModulePath = Directory.GetCurrentDirectory(),
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
            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions
            {
                ModulePath = @"C:\",
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
            var ex = await Assert.ThrowsAsync<ModuleLoaderInternalException>(() => moduleLoader.RestorePackages());

            // ASSERT
            Assert.NotNull(ex);
        }

        [Fact]
        public async Task WhenRestorePackagesAndNoProjectExistsThenExceptionIsThrown()
        {
            // ARRANGE
            var currentDirectory = Directory.GetCurrentDirectory();
            if (File.Exists(Path.Combine(currentDirectory, $"confs\\config.template.config")))
            {
                File.Delete(Path.Combine(currentDirectory, $"confs\\config.template.config"));
            }

            var nugetClientMock = new Mock<INugetClient>();
            var projectClientMock = new Mock<IProjectClient>();
            var moduleFeedClientMock = new Mock<IModuleFeedClient>();
            var moduleFeedClientFactoryMock = new Mock<IModuleFeedClientFactory>();
            projectClientMock.Setup(d => d.Download(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<Stream>(new FileStream("config.template.config", FileMode.Open, FileAccess.Read)));
            moduleFeedClientMock.Setup(m => m.GetProjectClient()).Returns(projectClientMock.Object);
            moduleFeedClientFactoryMock.Setup(m => m.BuildModuleFeedClient()).Returns(moduleFeedClientMock.Object);
            var options = new ModuleLoaderOptions
            {
                ModulePath = Path.Combine(Directory.GetCurrentDirectory(), "confs"),
                NugetSources = new[] { "nuget" },
                ProjectName = "projectName",
                ModuleFeedUri = new Uri("http://localhost/configuration")
            };

            // ACT
            var moduleLoader = new ModuleLoader(nugetClientMock.Object, moduleFeedClientFactoryMock.Object, options);
            moduleLoader.Initialize();
            var exception = await Assert.ThrowsAsync<ModuleLoaderInternalException>(() => moduleLoader.RestorePackages());

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal($"The project {options.ProjectName} doesn't exist", exception.Message);
        }
        
        #endregion

        #region CheckConfigurationFile

        [Fact]
        public void WhenCheckConfigurationFileThenExceptionIsThrown()
        {
            // ARRANGE
            var nugetClientMock = new Mock<INugetClient>();
            var moduleFeedClientFactoryMock = new Mock<IModuleFeedClientFactory>();
            var options = new ModuleLoaderOptions
            {
                ModulePath = Directory.GetCurrentDirectory(),
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
            Assert.Equal(5, exception.Messages.Count());
        }

        #endregion
    }
}