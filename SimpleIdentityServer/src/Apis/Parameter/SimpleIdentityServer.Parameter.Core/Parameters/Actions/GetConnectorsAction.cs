using SimpleIdentityServer.Parameter.Core.Common;
using SimpleIdentityServer.Parameter.Core.Responses;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IGetConnectorsAction
    {
        GetConnectorsResponse Execute();
    }

    internal class GetConnectorsAction : IGetConnectorsAction
    {
        private readonly IGetProjectConfiguration _getProjectConfiguration;

        public GetConnectorsAction(IGetProjectConfiguration getProjectConfiguration)
        {
            _getProjectConfiguration = getProjectConfiguration;
        }

        public GetConnectorsResponse Execute()
        {
            var result = _getProjectConfiguration.Execute();
            return new GetConnectorsResponse
            {
                Connectors = result.Key.Connectors,
                TemplateConnectors = result.Value.Connectors
            };
        }
    }
}
