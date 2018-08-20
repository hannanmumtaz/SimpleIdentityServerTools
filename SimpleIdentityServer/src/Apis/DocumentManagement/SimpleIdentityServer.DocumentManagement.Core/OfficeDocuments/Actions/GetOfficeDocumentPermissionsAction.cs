using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGetOfficeDocumentPermissionsAction
    {
        Task<IEnumerable<OfficeDocumentPermissionResponse>> Execute(string documentId, string subject, AuthenticateParameter authenticateParameter);
    }

    internal sealed class GetOfficeDocumentPermissionsAction : IGetOfficeDocumentPermissionsAction
    {
        private readonly IGetOfficeDocumentAction _getOfficeDocumentAction;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IAccessTokenStore _accessTokenStore;

        public GetOfficeDocumentPermissionsAction(IGetOfficeDocumentAction getOfficeDocumentAction, IIdentityServerUmaClientFactory identityServerUmaClientFactory, IAccessTokenStore accessTokenStore)
        {
            _getOfficeDocumentAction = getOfficeDocumentAction;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _accessTokenStore = accessTokenStore;
        }

        public async Task<IEnumerable<OfficeDocumentPermissionResponse>> Execute(string documentId, string subject, AuthenticateParameter authenticateParameter)
        {
            if(string.IsNullOrWhiteSpace(subject))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMissing);
            }

            var officeDocument = await _getOfficeDocumentAction.Execute(documentId);
            if(officeDocument.Subject != subject)
            {
                throw new NotAuthorizedException(ErrorCodes.Authorization, ErrorDescriptions.NotAuthorized);
            }

            var grantedToken = await _accessTokenStore.GetToken(authenticateParameter.WellKnownConfigurationUrl, authenticateParameter.ClientId, authenticateParameter.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotRetrieveAccessToken);
            }
            
            var policy = await _identityServerUmaClientFactory.GetPolicyClient().GetByResolution(officeDocument.UmaPolicyId, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (policy.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotGetUmaPolicy);
            }

            var result = new List<OfficeDocumentPermissionResponse>();
            if (policy.Content != null && policy.Content.Rules != null)
            {
                foreach(var rule in policy.Content.Rules)
                {
                    if (rule.Claims != null)
                    {
                        foreach(var claim in rule.Claims.Where(c => c.Type == "sub"))
                        {
                            result.Add(new OfficeDocumentPermissionResponse
                            {
                                UserSubject = claim.Value
                            });
                        }
                    }
                }
            }

            return result;
        }
    }
}
