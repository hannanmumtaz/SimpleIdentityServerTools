using Moq;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using SimpleIdentityServer.Parameter.Core.Params;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace SimpleIdentityServer.Parameter.Core.Tests.Parameters.Actions
{
    public class UpdateModuleConfigurationActionFixture
    {
        private const string _subPath = "UpdateModuleConfigurationActionFixture";
        private Mock<IDirectoryHelper> _directoryHelperStub;
        private IUpdateModuleConfigurationAction _updateModuleConfigurationAction;

        [Fact]
        public void WhenPassNullParameterThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();

            // ACT & ASSERT
            Assert.Throws<ArgumentNullException>(() => _updateModuleConfigurationAction.Execute(null));
        }

        [Fact]
        public void WhenUpdateRequestIsInvalidThenExceptionIsThrown()
        {
            // ARRANGE
            InitializeFakeObjects();
            RemoveConfigurationFiles();
            AddValidConfigurationFile();
            AddValidConfigurationTemplateFile();
            
            // ACT
            var invalidUnitException = Assert.Throws<ParameterAggregateException>(() => _updateModuleConfigurationAction.Execute(new[] { new UpdateParameter
            {
                UnitName = "invalidunit"
            }}));
            var invalidCategoryException = Assert.Throws<ParameterAggregateException>(() => _updateModuleConfigurationAction.Execute(new[] { new UpdateParameter
            {
                UnitName = "umahost",
                CategoryName = "invalidcategory"
            }}));
            var invalidLibraryException = Assert.Throws<ParameterAggregateException>(() => _updateModuleConfigurationAction.Execute(new[] { new UpdateParameter
            {
                UnitName = "umahost",
                CategoryName = "host",
                LibraryName = "invalidlib"
            }}));
            var invalidParameterException = Assert.Throws<ParameterAggregateException>(() => _updateModuleConfigurationAction.Execute(new[] { new UpdateParameter
            {
                UnitName = "umahost",
                LibraryName = "firstPackage",
                CategoryName = "host",
                Parameters = new Dictionary<string, string>
                {
                    { "fourthKey", "value" }
                }
            }}));

            Assert.NotNull(invalidUnitException);
            Assert.NotNull(invalidCategoryException);
            Assert.NotNull(invalidLibraryException);
            Assert.NotNull(invalidParameterException);
            Assert.True(invalidUnitException.Messages.Contains("The unit invalidunit doesn't exist"));
            Assert.True(invalidCategoryException.Messages.Contains("The category umahost\\invalidcategory doesn't exist"));
            Assert.True(invalidLibraryException.Messages.Contains("The library invalidlib doesn't exist in the unit umahost"));
            Assert.True(invalidParameterException.Messages.Contains("The parameter fourthKey doesn't exist in the package firstPackage\\host"));
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
                File.WriteAllText(configurationFilePath, "{ \"units\" : [ ] }");
            }
        }

        private static void AddInvalidConfigurationTemplateFile()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.template.config");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "{ \"units\" : [ ] }");
            }
        }

        private static void AddValidConfigurationFile()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.json");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "{" +
                    "\"units\" : [" +
                        "{"+
                            "\"name\" : \"umahost\"," +
                            "\"packages\" : ["+
                                "{" +
                                    "\"lib\" : \"firstPackage\"," +
                                    "\"category\" : \"host\"," +
                                    "\"parameters\" : { " +
                                        "\"firstKey\" : \"value\"," +
                                        "\"secondKey\" : \"value\"," +
                                        "\"thirdKey\" : \"value\"," +
                                    "}" +
                                "}" +
                            "]"+
                        "}" +
                    "]" +
                    "}");
            }
        }

        private static void AddValidConfigurationTemplateFile()
        {
            var configurationFilePath = Path.Combine(Directory.GetCurrentDirectory(), _subPath, "config.template.config");
            if (!File.Exists(configurationFilePath))
            {
                File.WriteAllText(configurationFilePath, "{" +
                    "\"units\" : [" +
                        "{" +
                            "\"name\" : \"umahost\"," +
                            "\"packages\" : [" +
                                "{" +
                                    "\"lib\" : \"firstPackage\"," +
                                    "\"category\" : \"host\"," +
                                    "\"parameters\" : { " +
                                        "\"firstKey\" : \"\"," +
                                        "\"secondKey\" : \"\"," +
                                        "\"thirdKey\" : \"\"," +
                                    "}" +
                                "}" +
                            "]" +
                        "}" +
                    "]" +
                    "}");
            }
        }

        private void InitializeFakeObjects()
        {
            _directoryHelperStub = new Mock<IDirectoryHelper>();
            _directoryHelperStub.Setup(d => d.GetCurrentDirectory()).Returns(Path.Combine(Directory.GetCurrentDirectory(), _subPath));
            _updateModuleConfigurationAction = new UpdateUnitsAction(_directoryHelperStub.Object);
        }
    }
}
