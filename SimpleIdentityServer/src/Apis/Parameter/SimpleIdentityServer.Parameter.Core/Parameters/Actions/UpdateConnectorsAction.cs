using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Params;
using System;
using System.Collections.Generic;
using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IUpdateConnectorsAction
    {
        bool Execute(IEnumerable<UpdateConnector> updateConnectors);
    }

    internal sealed class UpdateConnectorsAction : IUpdateConnectorsAction
    {
        private readonly IDirectoryHelper _directoryHelper;

        public UpdateConnectorsAction(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public bool Execute(IEnumerable<UpdateConnector> updateConnectors)
        {
            if (updateConnectors == null)
            {
                throw new ArgumentNullException(nameof(updateConnectors));
            }
            
            var currentDirectory = _directoryHelper.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationPath));
            var connectors = new List<ProjectConnectorResponse>();
            foreach(var updateConnector in updateConnectors)
            {
                connectors.Add(new ProjectConnectorResponse
                {
                    Library = updateConnector.Library,
                    Name = updateConnector.Name,
                    Parameters = updateConnector.Parameters,
                    Version = updateConnector.Version
                });
            }

            projectConfiguration.Connectors = connectors;
            var json = JsonConvert.SerializeObject(projectConfiguration);
            File.WriteAllText(configurationPath, json);
            return true;
        }
    }
}