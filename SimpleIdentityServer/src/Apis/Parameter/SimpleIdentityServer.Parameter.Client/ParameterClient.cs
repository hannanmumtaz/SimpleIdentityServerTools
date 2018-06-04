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
        Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null);
    }

    internal sealed class ParameterClient : IParameterClient
    {
        private readonly IUpdateModulesAction _updateModulesAction;
        private readonly IGetModulesAction _getModulesAction;

        public ParameterClient(IUpdateModulesAction updateModulesAction, IGetModulesAction getModulesAction)
        {
            _updateModulesAction = updateModulesAction;
            _getModulesAction = getModulesAction;
        }

        public Task<GetModulesResult> Get(string baseUrl, string accessToken = null)
        {
            return _getModulesAction.Execute(baseUrl, accessToken);
        }

        public Task<ErrorResponse> Update(string baseUrl, IEnumerable<UpdateParameterRequest> parameters, string accessToken = null)
        {
            return _updateModulesAction.Execute(baseUrl, parameters, accessToken);
        }
    }
}