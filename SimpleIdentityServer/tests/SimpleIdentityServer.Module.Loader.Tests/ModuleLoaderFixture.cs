using Moq;
using SimpleIdentityServer.Module.Feed.Client;
using SimpleIdentityServer.Module.Loader.Exceptions;
using System;
using System.IO;
using System.Linq;
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
            Assert.Throws<ArgumentNullException>(() => new ModuleLoader(null));
        }

        #region Initialize
        
        [Fact]
        public void WhenInitializeModuleLoaderAndProjectNameIsNullThenExceptionIsThrown()
        {
            // ARRANGE
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions
            {
                ProjectName = null
            };
            var moduleLoader = new ModuleLoader(options);

            // ACT
            var exception = Assert.Throws<ModuleLoaderConfigurationException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
            Assert.Equal("The project name must be specified", exception.Message);
        }
        
        [Fact]
        public void WhenInitializeModuleLoaderAndConfigurationFileDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            RemoveFiles();
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            var options = new ModuleLoaderOptions
            {
                ProjectName = "projectName"
            };
            var moduleLoader = new ModuleLoader(options);

            // ACT
            var exception = Assert.Throws<FileNotFoundException>(() => moduleLoader.Initialize());

            // ASSERT
            Assert.NotNull(exception);
        }

        #endregion

        #region CheckConfigurationFile

        [Fact]
        public void WhenCheckConfigurationFileThenExceptionIsThrown()
        {
            // TODO : FIX THIS ISSUE : Assembly.GetEntryAssembly().Location
            // ARRANGE
            RemoveFiles();
            AddTemplateFile();
            AddConfigFile();
            var moduleFeedClientFactoryMock = new Mock<IModuleFeedClientFactory>();
            var options = new ModuleLoaderOptions
            {
                ProjectName = "projectName"
            };

            // ACT
            var moduleLoader = new ModuleLoader(options);
            moduleLoader.Initialize();
            var exception = Assert.Throws<ModuleLoaderAggregateConfigurationException>(() => moduleLoader.CheckConfigurationFile());

            // ASSERTS
            Assert.NotNull(exception);
            Assert.Equal(4, exception.Messages.Count());
        }

        #endregion

        private void RemoveFiles()
        {
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.json")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "config.json"));
            }
            
            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "config.template.config")))
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "config.template.config"));
            }
        }

        private void AddTemplateFile()
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.template.config"), _templateJson);
        }

        private void AddConfigFile()
        {
            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "config.json"), _configJson);
        }
    }
}