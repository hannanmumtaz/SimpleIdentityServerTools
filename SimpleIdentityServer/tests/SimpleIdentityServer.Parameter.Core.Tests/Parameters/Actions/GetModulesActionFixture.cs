using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using System.IO;
using Xunit;

namespace SimpleIdentityServer.Parameter.Core.Tests.Parameters.Actions
{
    public class GetModulesActionFixture
    {
        [Fact]
        public void WhenConfigurationFileDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            RemoveConfigurationFiles();
            var getModulesAction = new GetModulesAction();

            // ACT & ASSERT
            Assert.Throws<ConfigurationNotFoundException>(() => getModulesAction.Execute());
        }

        [Fact]
        public void WhenConfigurationFileTemplateDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            RemoveConfigurationFiles();
            AddInvalidConfigurationFile();
            var getModulesAction = new GetModulesAction();

            // ACT & ASSERT
            Assert.Throws<NotRestoredException>(() => getModulesAction.Execute());
        }

        [Fact]
        public void WhenConfigurationFileIsNotCorrectThenExceptionIsThrown()
        {
            // ARRANGE
            RemoveConfigurationFiles();
            AddInvalidConfigurationFile();
            AddInvalidConfigurationTemplateFile();
            var getModulesAction = new GetModulesAction();

            // ACT & ASSERT
            Assert.Throws<BadConfigurationException>(() => getModulesAction.Execute());
        }

        private static void RemoveConfigurationFiles()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            var configurationTemplateFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.template.config");
            if (File.Exists(configurationFilePath))
            {
                File.Delete(configurationFilePath);
            }

            if (File.Exists(configurationTemplateFilePath))
            {
                File.Delete(configurationTemplateFilePath);
            }
        }

        private static void AddInvalidConfigurationFile()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "abcd");
            }
        }

        private static void AddInvalidConfigurationTemplateFile()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), "config.template.config");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "abcd");
            }
        }
    }
}
