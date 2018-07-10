using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using System.Collections.Generic;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Common
{
    public interface IGetProjectConfiguration
    {
        KeyValuePair<ProjectResponse, ProjectResponse> Execute();
    }

    internal sealed class GetProjectConfiguration
    {
        private readonly IDirectoryHelper _directoryHelper;

        public GetProjectConfiguration(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public KeyValuePair<ProjectResponse, ProjectResponse> Execute()
        {
            var currentDirectory = _directoryHelper.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var configurationTemplatePath = Path.Combine(currentDirectory, Constants.ConfigurationTemplateFileName);
            if (!File.Exists(configurationPath))
            {
                throw new ConfigurationNotFoundException();
            }

            if (!File.Exists(configurationTemplatePath))
            {
                throw new NotRestoredException();
            }

            var projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationPath));
            if (projectConfiguration == null)
            {
                throw new BadConfigurationException($"the {Constants.ConfigurationFileName} is not correct");
            }

            var projectTemplateConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationTemplatePath));
            if (projectTemplateConfiguration == null)
            {
                throw new BadConfigurationException($"The {Constants.ConfigurationTemplateFileName} is not correct");
            }

            return new KeyValuePair<ProjectResponse, ProjectResponse>(projectConfiguration, projectTemplateConfiguration);
        }
    }
}
