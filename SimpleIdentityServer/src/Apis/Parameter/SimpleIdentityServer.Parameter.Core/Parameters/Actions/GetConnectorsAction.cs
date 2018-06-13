using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Helpers;
using System.Collections.Generic;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IGetConnectorsAction
    {
        IEnumerable<ProjectConnectorResponse> Execute();
    }

    internal class GetConnectorsAction : IGetConnectorsAction
    {
        private readonly IDirectoryHelper _directoryHelper;

        public GetConnectorsAction(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public IEnumerable<ProjectConnectorResponse> Execute()
        {
            var currentDirectory = _directoryHelper.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationPath));
            return projectConfiguration.Connectors;
        }
    }
}
