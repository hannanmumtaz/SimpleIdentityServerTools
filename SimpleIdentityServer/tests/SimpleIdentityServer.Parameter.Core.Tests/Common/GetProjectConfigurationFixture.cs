using Moq;
using SimpleIdentityServer.Parameter.Core.Common;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using System.IO;
using Xunit;

namespace SimpleIdentityServer.Parameter.Core.Tests.Common
{
    public class GetProjectConfigurationFixture
    {
        private const string _subPath = "GetProjectConfigurationFixture";
        private Mock<IDirectoryHelper> _directoryHelperStub;
        private IGetProjectConfiguration _getProjectConfiguration;

        [Fact]
        public void WhenConfigurationFileDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            RemoveConfigurationFiles();

            // ACT & ASSERT
            Assert.Throws<ConfigurationNotFoundException>(() => _getProjectConfiguration.Execute());
        }

        [Fact]
        public void WhenConfigurationFileTemplateDoesntExistThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            RemoveConfigurationFiles();
            AddInvalidConfigurationFile();

            // ACT & ASSERT
            Assert.Throws<NotRestoredException>(() => _getProjectConfiguration.Execute());
        }

        private static void RemoveConfigurationFiles()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.json");
            var configurationTemplateFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.template.config");
            if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), _subPath)))
            {
                Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), _subPath));
            }

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
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.json");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "abcd");
            }
        }

        private static void AddInvalidConfigurationTemplateFile()
        {
            var configurationTemplateFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.template.config");
            if (!File.Exists(configurationTemplateFilePath))
            {
                File.WriteAllText(configurationTemplateFilePath, "abcd");
            }
        }

        private void InitializeFakeObjects()
        {
            _directoryHelperStub = new Mock<IDirectoryHelper>();
            _directoryHelperStub.Setup(d => d.GetCurrentDirectory()).Returns(Path.Combine(Directory.GetCurrentDirectory(), _subPath));
            _getProjectConfiguration = new GetProjectConfiguration(_directoryHelperStub.Object);
        }
    }
}
