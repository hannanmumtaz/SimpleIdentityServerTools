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
        Task<GetModulesResult> Get(string subject, string type);
    }

    internal sealed class ParameterActions : IParameterActions
    {
        private readonly IGetParametersAction _getParametersAction;
        private readonly IUpdateParametersAction _updateParametersAction;

        public ParameterActions(IGetParametersAction getParametersAction, IUpdateParametersAction updateParametersAction)
        {
            _getParametersAction = getParametersAction;
            _updateParametersAction = updateParametersAction;  
        }

        public Task<bool> Update(string subject, IEnumerable<UpdateParameterRequest> updateParameters, string type)
        {
            return _updateParametersAction.Execute(subject, updateParameters, type);
        }

        public Task<GetModulesResult> Get(string subject, string type)
        {
            return _getParametersAction.Execute(subject, type);
        }
    }
}
