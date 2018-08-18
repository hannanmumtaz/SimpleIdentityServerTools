using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using SimpleIdentityServer.DocumentManagement.Store;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IValidateConfirmationLinkAction
    {
        Task<bool> Execute(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter);
    }

    internal sealed class ValidateConfirmationLinkAction : IValidateConfirmationLinkAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;
        private readonly IValidateConfirmationLinkParameterValidator _validateConfirmationLinkParameterValidator;
        private readonly IAccessTokenStore _tokenStore;

        public ValidateConfirmationLinkAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory,
            IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore,
            IValidateConfirmationLinkParameterValidator validateConfirmationLinkParameterValidator, IAccessTokenStore tokenStore)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
            _validateConfirmationLinkParameterValidator = validateConfirmationLinkParameterValidator;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string wellKnownConfiguration, ValidateConfirmationLinkParameter validateConfirmationLinkParameter, AuthenticateParameter authenticateParameter)
        {
            if (string.IsNullOrWhiteSpace(wellKnownConfiguration))
            {
                throw new ArgumentNullException(nameof(wellKnownConfiguration));
            }

            _validateConfirmationLinkParameterValidator.Check(validateConfirmationLinkParameter);
            var confirmationLink = await _officeDocumentConfirmationLinkStore.Get(validateConfirmationLinkParameter.ConfirmationCode);
            if (confirmationLink == null)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.TheConfirmationCodeIsNotValid);
            }

            CheckConfirmationLink(confirmationLink);
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

            var putPolicyRules = new List<PutPolicyRule>();
            if (policy.Content.Rules != null)
            {
                foreach(var rule in policy.Content.Rules)
                {
                    putPolicyRules.Add(new PutPolicyRule
                    {
                        Id = rule.Id,
                        Claims = rule.Claims,
                        ClientIdsAllowed = rule.ClientIdsAllowed,
                        OpenIdProvider = rule.OpenIdProvider,
                        Scopes = rule.Scopes
                    });
                }
            }

            if (!putPolicyRules.Any(p => p.Claims != null && p.Claims.Any(c => c.Type == "sub" && c.Value == validateConfirmationLinkParameter.Subject)))
            {
                putPolicyRules.Add(new PutPolicyRule
                {
                    Claims = new List<PostClaim>
                    {
                        new PostClaim
                        {
                            Type = "sub",
                            Value = validateConfirmationLinkParameter.Subject
                        }
                    },
                    OpenIdProvider = wellKnownConfiguration,
                    Scopes = Constants.DEFAULT_SCOPES.ToList()
                });
            }

            var updatedResult = await _identityServerUmaClientFactory.GetPolicyClient().UpdateByResolution(new PutPolicy
            {
                PolicyId = policy.Content.Id,
                Rules = putPolicyRules
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (updatedResult.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.UmaPolicyCannotBeUpdated);
            }

            // TODO : DECREASE THE NUMBER OF CONFIRMATION LINKS
            await UseConfirmationLink(confirmationLink);
            return true;
        }

        /// <summary>
        /// Check the confirmation link is valid.
        /// </summary>
        /// <param name="officeDocumentConfirmationLink"></param>
        private void CheckConfirmationLink(OfficeDocumentConfirmationLink officeDocumentConfirmationLink)
        {
            if (officeDocumentConfirmationLink.ExpiresIn != null)
            {
                if (DateTime.UtcNow >= officeDocumentConfirmationLink.CreateDateTime.AddSeconds(officeDocumentConfirmationLink.ExpiresIn.Value))
                {
                    throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.ConfirmationCodeIsExpired);
                }
            }

            if (officeDocumentConfirmationLink.NumberOfConfirmations != null && officeDocumentConfirmationLink.NumberOfConfirmations.Value == 0)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.NotEnoughConfirmationCode);
            }
        }

        /// <summary>
        /// Use the confirmation code.
        /// </summary>
        /// <param name="officeDocumentConfirmationLink"></param>
        /// <returns></returns>
        private async Task UseConfirmationLink(OfficeDocumentConfirmationLink officeDocumentConfirmationLink)
        {
            if(officeDocumentConfirmationLink.NumberOfConfirmations != null)
            {
                var nbConfirmations = officeDocumentConfirmationLink.NumberOfConfirmations.Value;
                nbConfirmations--;
                officeDocumentConfirmationLink.NumberOfConfirmations = nbConfirmations;
                if(nbConfirmations == 0)
                {
                    await _officeDocumentConfirmationLinkStore.Remove(officeDocumentConfirmationLink.ConfirmationCode);
                    return;
                }

                await _officeDocumentConfirmationLinkStore.Update(officeDocumentConfirmationLink);
                return;
            }

            await _officeDocumentConfirmationLinkStore.Remove(officeDocumentConfirmationLink.ConfirmationCode);
        }
    }
}
