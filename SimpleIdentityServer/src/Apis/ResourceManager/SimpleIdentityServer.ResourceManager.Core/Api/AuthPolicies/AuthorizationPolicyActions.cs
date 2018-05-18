using SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies.Actions;
using SimpleIdentityServer.Uma.Common.DTOs;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies
{
    public interface IAuthorizationPolicyActions
    {
        Task<bool> Delete(string subject, string policyId);
        Task<AddPolicyResponse> Add(string subject, PostPolicy postPolicy);
        Task<bool> Update(string subject, PutPolicy putPolicy);
    }

    internal sealed class AuthorizationPolicyActions : IAuthorizationPolicyActions
    {
        private readonly IUpdateAuthorizationPolicyAction _updateAuthorizationPolicyAction;
        private readonly IAddAuthorizationPolicyAction _addAuthorizationPolicyAction;
        private readonly IDeleteAuthorizationPolicyAction _deleteAuthorizationPolicyAction;

        public AuthorizationPolicyActions(IUpdateAuthorizationPolicyAction updateAuthorizationPolicyAction, IAddAuthorizationPolicyAction addAuthorizationPolicyAction, IDeleteAuthorizationPolicyAction deleteAuthorizationPolicyAction)
        {
            _updateAuthorizationPolicyAction = updateAuthorizationPolicyAction;
            _addAuthorizationPolicyAction = addAuthorizationPolicyAction;
            _deleteAuthorizationPolicyAction = deleteAuthorizationPolicyAction;
        }

        public Task<bool> Delete(string subject, string policyId)
        {
            return _deleteAuthorizationPolicyAction.Execute(subject, policyId);
        }

        public Task<AddPolicyResponse> Add(string subject, PostPolicy postPolicy)
        {
            return _addAuthorizationPolicyAction.Execute(subject, postPolicy);
        }

        public Task<bool> Update(string subject, PutPolicy putPolicy)
        {
            return _updateAuthorizationPolicyAction.Execute(subject, putPolicy);
        }
    }
}
