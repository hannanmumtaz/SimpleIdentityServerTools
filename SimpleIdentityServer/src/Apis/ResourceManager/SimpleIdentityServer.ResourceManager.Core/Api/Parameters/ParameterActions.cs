using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using SimpleIdentityServer.ResourceManager.Core.Api.Parameters.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Parameters
{
    public interface IParameterActions
    {
        Task<bool> Update(string subject, IEnumerable<UpdateParameterRequest> updateParameters, string type);
	Task<bool> UpdateConnectors(string subject, IEnumerable<UpdateConnectorRequest> updateParameters);
        Task<GetModulesResult> Get(string subject, string type);
	Task<GetConnectorsResult> GetConnectors(string subject);
    }

    internal sealed class ParameterActions : IParameterActions
    {
        private readonly IGetParametersAction _getParametersAction;
        private readonly IGetConnectorsAction _getConnectorsAction;
        private readonly IUpdateParametersAction _updateParametersAction;
        private readonly IUpdateConnectorsAction _updateConnectorsAction;

        public ParameterActions(IGetParametersAction getParametersAction, IGetConnectorsAction getConnectorsAction, IUpdateParametersAction updateParametersAction, IUpdateConnectorsAction updateConnectorsAction)
        {
            _getParametersAction = getParametersAction;
            _getConnectorsAction = getConnectorsAction;
            _updateParametersAction = updateParametersAction;
            _updateConnectorsAction = updateConnectorsAction;
        }

        public Task<bool> Update(string subject, IEnumerable<UpdateParameterRequest> updateParameters, string type)
        {
            return _updateParametersAction.Execute(subject, updateParameters, type);
        }

        public Task<bool> UpdateConnectors(string subject, IEnumerable<UpdateConnectorRequest> updateParameters)
        {
            return _updateConnectorsAction.Execute(subject, updateParameters);
        }

        public Task<GetModulesResult> Get(string subject, string type)
        {
            return _getParametersAction.Execute(subject, type);
        }

        public Task<GetConnectorsResult> GetConnectors(string subject)
        {
            return _getConnectorsAction.Execute(subject);
        }
    }
}
