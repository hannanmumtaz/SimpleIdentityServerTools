using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Client
{
    public interface IParameterClient
    {
        Task<GetUnitsResult> Get(string baseUrl, string accessToken = null);
        Task<GetConnectorsResult> GetConnectors(string baseUrl, string accessToken = null);
        Task<GetTwoFactorsResult> GetTwoFactors(string baseUrl, string accessToken = null);
        Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null);
        Task<ErrorResponse> UpdateConnectors(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null);
        Task<ErrorResponse> UpdateTwoFactors(string baseUrl, IEnumerable<UpdateTwoFactorRequest> parameters, string accessToken = null);
    }

    internal sealed class ParameterClient : IParameterClient
    {
        private readonly IGetUnitsAction _getUnitsAction;
        private readonly IGetConnectorsAction _getConnectorsAction;
        private readonly IGetTwoFactorsAction _getTwoFactorsAction;
        private readonly IUpdateUnitsAction _updateUnitsAction;
        private readonly IUpdateConnectorsAction _updateConnectorsAction;
        private readonly IUpdateTwoFactorsAction _updateTwoFactorsAction;

        public ParameterClient(IGetUnitsAction getUnitsAction, IGetConnectorsAction getConnectorsAction, IGetTwoFactorsAction getTwoFactorsAction,
            IUpdateUnitsAction updateModulesAction, IUpdateConnectorsAction updateConnectorsAction, IUpdateTwoFactorsAction updateTwoFactorsAction)
        {
            _getUnitsAction = getUnitsAction;
            _getConnectorsAction = getConnectorsAction;
            _getTwoFactorsAction = getTwoFactorsAction;
            _updateUnitsAction = updateModulesAction;
            _updateConnectorsAction = updateConnectorsAction;
            _updateTwoFactorsAction = updateTwoFactorsAction;
        }

        public Task<GetUnitsResult> Get(string baseUrl, string accessToken = null)
        {
            return _getUnitsAction.Execute(baseUrl, accessToken);
        }

        public Task<GetConnectorsResult> GetConnectors(string baseUrl, string accessToken = null)
        {
            return _getConnectorsAction.Execute(baseUrl, accessToken);
        }

        public Task<GetTwoFactorsResult> GetTwoFactors(string baseUrl, string accessToken = null)
        {
            return _getTwoFactorsAction.Execute(baseUrl, accessToken);
        }

        public Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null)
        {
            return _updateUnitsAction.Execute(baseUrl, parameters, accessToken);
        }

        public Task<ErrorResponse> UpdateConnectors(string baseUrl, IEnumerable<UpdateConnectorRequest> parameters, string accessToken = null)
        {
            return _updateConnectorsAction.Execute(baseUrl, parameters, accessToken);
        }

        public Task<ErrorResponse> UpdateTwoFactors(string baseUrl, IEnumerable<UpdateTwoFactorRequest> parameters, string accessToken = null)
        {
            return _updateTwoFactorsAction.Execute(baseUrl, parameters, accessToken);
        }
    }
}