using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
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
    public interface IAddOfficeDocumentAction
    {
        Task<bool> Execute(string openIdWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter);
    }

    internal sealed class AddOfficeDocumentAction : IAddOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly IAccessTokenStore _accessTokenStore;
        private readonly IAddDocumentParameterValidator _addDocumentParameterValidator;

        public AddOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory, IAccessTokenStore accessTokenStore,
            IAddDocumentParameterValidator addDocumentParameterValidator)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _accessTokenStore = accessTokenStore;
            _addDocumentParameterValidator = addDocumentParameterValidator;
        }

        public async Task<bool> Execute(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter)
        {
            if (string.IsNullOrWhiteSpace(openidWellKnownConfiguration))
            {
                throw new ArgumentNullException(nameof(openidWellKnownConfiguration));
            }

            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            _addDocumentParameterValidator.Check(document);
            var officeDocument = await _officeDocumentRepository.Get(document.Id);
            if (officeDocument != null)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.OfficeDocumentExists);
            }

            var grantedToken = await _accessTokenStore.GetToken(authenticateParameter.WellKnownConfigurationUrl, authenticateParameter.ClientId, authenticateParameter.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotRetrieveAccessToken);
            }

            var resource = await _identityServerUmaClientFactory.GetResourceSetClient().AddByResolution(new PostResourceSet
            {
                Name = $"officedocument_{document.Id}",
                Scopes = Constants.DEFAULT_SCOPES.ToList()
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (resource.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotAddUmaResource);
            }

            var policy = await _identityServerUmaClientFactory.GetPolicyClient().AddByResolution(new PostPolicy
            {
                ResourceSetIds = new List<string> { resource.Content.Id },
                Rules = new List<PostPolicyRule>
                {
                    new PostPolicyRule
                    {
                        Claims = new List<PostClaim>
                        {
                            new PostClaim
                            {
                                Type = "sub",
                                Value = document.Subject
                            }
                        },
                        Scopes = Constants.DEFAULT_SCOPES.ToList(),
                        OpenIdProvider = openidWellKnownConfiguration
                    }
                }
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (policy.ContainsError)
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotAddUmaPolicy);
            }

            officeDocument = new OfficeDocumentAggregate
            {
                Id = document.Id,
                Subject = document.Subject,
                UmaResourceId = resource.Content.Id,
                UmaPolicyId = policy.Content.PolicyId,
            };
            if (!await _officeDocumentRepository.Add(officeDocument))
            {
                throw new BaseDocumentManagementApiException(ErrorCodes.InternalError, ErrorDescriptions.CannotAddOfficeDocument);
            }

            return true;
        }
    }
}
