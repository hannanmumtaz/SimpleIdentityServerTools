using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using SimpleIdentityServer.DocumentManagement.Store;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IValidateConfirmationLinkAction
    {

    }

    internal sealed class ValidateConfirmationLinkAction : IValidateConfirmationLinkAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;
        private readonly IAccessTokenStore _tokenStore;

        public ValidateConfirmationLinkAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory,
            IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore, IAccessTokenStore tokenStore)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter)
        {
            var confirmationLink = await _officeDocumentConfirmationLinkStore.Get(validateConfirmationLinkParameter.ConfirmationCode);
            if (confirmationLink == null)
            {
                // TODO : THROW EXCEPTION.
            }

            var officeDocument = await _officeDocumentRepository.Get(confirmationLink.DocumentId);
            if (officeDocument == null)
            {
                throw new DocumentNotFoundException();
            }

            if (string.IsNullOrWhiteSpace(officeDocument.UmaResourceId))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.NoUmaResource);
            }

            if (string.IsNullOrWhiteSpace(officeDocument.UmaPolicyId))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.NoUmaPolicy);
            }

            var grantedToken = await _tokenStore.GetToken(authenticateParameter.WellKnownConfigurationUrl, authenticateParameter.ClientId, authenticateParameter.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotRetrieveAccessToken);
            }

            var policy = await _identityServerUmaClientFactory.GetPolicyClient().GetByResolution(officeDocument.UmaPolicyId, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (policy.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.UmaPolicyDoesntExist);
            }

            // policy.Content.Rules.
            // TODO : Add policy.
            // Update the policy.
            return true;
        }
    }
}
