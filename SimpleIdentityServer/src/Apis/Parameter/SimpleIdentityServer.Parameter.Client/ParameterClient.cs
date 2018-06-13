using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Client
{
    public interface IParameterClient
    {
        Task<GetModulesResult> Get(string baseUrl, string accessToken = null);
        Task<GetConnectorsResult> GetConnectors(string baseUrl, string accessToken = null);
        Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null);
        Task<ErrorResponse> UpdateConnectors(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null);
    }

    internal sealed class ParameterClient : IParameterClient
    {
        private readonly IUpdateModulesAction _updateModulesAction;
        private readonly IGetModulesAction _getModulesAction;
        private readonly IUpdateConnectorsAction _updateConnectorsAction;
        private readonly IGetConnectorsAction _getConnectorsAction;

        public ParameterClient(IUpdateModulesAction updateModulesAction, IGetModulesAction getModulesAction, IGetConnectorsAction getConnectorsAction,
            IUpdateConnectorsAction updateConnectorsAction)
        {
            _updateModulesAction = updateModulesAction;
            _getModulesAction = getModulesAction;
            _updateConnectorsAction = updateConnectorsAction;
            _getConnectorsAction = getConnectorsAction;
        }

        public Task<GetModulesResult> Get(string baseUrl, string accessToken = null)
        {
            return _getModulesAction.Execute(baseUrl, accessToken);
        }

        public Task<GetConnectorsResult> GetConnectors(string baseUrl, string accessToken = null)
        {
            return _getConnectorsAction.Execute(baseUrl, accessToken);
        }

        public Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null)
        {
            return _updateModulesAction.Execute(baseUrl, parameters, accessToken);
        }

        public Task<ErrorResponse> UpdateConnectors(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null)
        {
            return _updateConnectorsAction.Execute(baseUrl, parameters, accessToken);
        }
    }
}