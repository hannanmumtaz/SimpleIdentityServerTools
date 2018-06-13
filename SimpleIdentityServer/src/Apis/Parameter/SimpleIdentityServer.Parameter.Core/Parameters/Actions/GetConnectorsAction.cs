using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Responses;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IGetConnectorsAction
    {
        GetConnectorsResponse Execute();
    }

    internal class GetConnectorsAction : IGetConnectorsAction
    {
        private readonly IDirectoryHelper _directoryHelper;

        public GetConnectorsAction(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public GetConnectorsResponse Execute()
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

            return new GetConnectorsResponse
            {
                Connectors = projectConfiguration.Connectors,
                TemplateConnectors = projectTemplateConfiguration.Connectors
            };
        }
    }
}
