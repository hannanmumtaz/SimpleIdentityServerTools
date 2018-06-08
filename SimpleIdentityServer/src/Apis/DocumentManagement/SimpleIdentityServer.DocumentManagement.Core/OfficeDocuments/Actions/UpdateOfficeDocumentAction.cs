using SimpleIdentityServer.Client;
using SimpleIdentityServer.Common.TokenStore;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IUpdateOfficeDocumentAction
    {
        Task<bool> Execute(string subject, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter);
    }

    internal sealed class UpdateOfficeDocumentAction : IUpdateOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ITokenStore _tokenStore;

        public UpdateOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory, ITokenStore tokenStore)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(string subject, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter)
        {
            if (string.IsNullOrWhiteSpace(subject))
            {
                throw new ArgumentNullException(nameof(subject));
            }

            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter.Permissions == null)
            {
                throw new ArgumentNullException(nameof(parameter.Permissions));
            }

            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            foreach (var permission in parameter.Permissions)
            {
                if (!permission.Scopes.Any(s => Constants.DEFAULT_SCOPES.Contains(s)))
                {
                    throw new InternalDocumentException("internal", "invalid_scopes");
                }

                if (string.IsNullOrWhiteSpace(permission.Subject))
                {
                    throw new InternalDocumentException("internal", "permission_subject_must_be_specified");
                }
            }

            var officeDocument = await _officeDocumentRepository.Get(documentId);
            if (officeDocument == null)
            {
                throw new InternalDocumentException("internal", "document_doesnt_exist");
            }
            
            if (string.IsNullOrWhiteSpace(officeDocument.UmaResourceId))
            {
                throw new InternalDocumentException("internal", "no_uma_resource");
            }

            if (string.IsNullOrWhiteSpace(officeDocument.UmaPolicyId))
            {
                throw new InternalDocumentException("internal", "no_uma_policy");
            }

            if (officeDocument.Subject != subject)
            {
                throw new NotAuthorizedException();
            }

            var grantedToken = await _tokenStore.GetToken(authenticateParameter.WellKnownConfigurationUrl, authenticateParameter.ClientId, authenticateParameter.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                throw new InvalidConfigurationException("invalid_client_configuration");
            }

            var policy = await _identityServerUmaClientFactory.GetPolicyClient().Get(officeDocument.UmaPolicyId, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (policy == null || string.IsNullOrWhiteSpace(policy.Id))
            {
                throw new InternalDocumentException("internal", "uma_policy_doesnt_exist");
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
                            Value = subject
                        }
                    },
                    Scopes = Constants.DEFAULT_SCOPES.ToList()
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
                    Scopes = permission.Scopes.ToList()
                });
            }
            if (!await _identityServerUmaClientFactory.GetPolicyClient().UpdateByResolution(new PutPolicy
            {
                PolicyId = policy.Id,
                Rules = putPolicyRules
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken))
            {
                throw new InternalDocumentException("internal", "policy_cannot_be_updated");
            }

            return true;
        }
    }
}
