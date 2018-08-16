using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.DocumentManagement.Core.Validators;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IUpdateOfficeDocumentAction
    {
        Task<bool> Execute(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter);
    }

    internal sealed class UpdateOfficeDocumentAction : IUpdateOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IAccessTokenStore _tokenStore;
        private readonly IUpdateOfficeDocumentParameterValidator _updateOfficeDocumentParameterValidator;

        public UpdateOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory, IAccessTokenStore tokenStore,
            IUpdateOfficeDocumentParameterValidator updateOfficeDocumentParameterValidator)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
            _updateOfficeDocumentParameterValidator = updateOfficeDocumentParameterValidator;
        }

        public async Task<bool> Execute(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter)
        {
            if (string.IsNullOrWhiteSpace(wellKnownConfiguration))
            {
                throw new ArgumentNullException(nameof(wellKnownConfiguration));
            }

            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            _updateOfficeDocumentParameterValidator.Check(parameter);
            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            foreach (var permission in parameter.Permissions)
            {
                if (!permission.Scopes.Any(s => Constants.DEFAULT_SCOPES.Contains(s)))
                {
                    throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.ScopesAreNotValid);
                }

                if (string.IsNullOrWhiteSpace(permission.Subject))
                {
                    throw new BaseDocumentManagementApiException(ErrorCodes.InvalidRequest, ErrorDescriptions.SubjectIsMandatoryInThePermission);
                }
            }

            var officeDocument = await _officeDocumentRepository.Get(documentId);
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

            if (officeDocument.Subject != parameter.Subject)
            {
                throw new NotAuthorizedException();
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

            var putPolicyRules = new List<PutPolicyRule>
            {
                new PutPolicyRule
                {
                    Claims = new List<PostClaim>
                    {
                        new PostClaim
                        {
                            Type = "sub",
                            Value = parameter.Subject
                        }
                    },
                    Scopes = Constants.DEFAULT_SCOPES.ToList(),
                    OpenIdProvider = wellKnownConfiguration
                }
            };
            foreach(var permission in parameter.Permissions)
            {
                putPolicyRules.Add(new PutPolicyRule
                {
                    Claims = new List<PostClaim>
                    {
                        new PostClaim
                        {
                            Type = "sub",
                            Value = permission.Subject
                        }
                    },
                    Scopes = permission.Scopes.ToList(),
                    OpenIdProvider = wellKnownConfiguration
                });
            }
            var updateResult = await _identityServerUmaClientFactory.GetPolicyClient().UpdateByResolution(new PutPolicy
            {
                PolicyId = policy.Content.Id,
                Rules = putPolicyRules
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (updateResult.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.UmaPolicyCannotBeUpdated);
            }

            return true;
        }
    }
}
