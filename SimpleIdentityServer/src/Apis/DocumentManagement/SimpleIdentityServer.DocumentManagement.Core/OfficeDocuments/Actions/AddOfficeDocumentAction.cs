using SimpleIdentityServer.Client;
using SimpleIdentityServer.Common.TokenStore;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Extensions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IAddOfficeDocumentAction
    {
        Task<bool> Execute(OfficeDocumentAggregate document, AuthenticateParameter authenticateParameter);
    }

    internal sealed class AddOfficeDocumentAction : IAddOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ITokenStore _tokenStore;

        public AddOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerUmaClientFactory identityServerUmaClientFactory, ITokenStore tokenStore)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _tokenStore = tokenStore;
        }

        public async Task<bool> Execute(OfficeDocumentAggregate document, AuthenticateParameter authenticateParameter)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            Check(document);
            var officeDocument = await _officeDocumentRepository.Get(document.Id);
            if (officeDocument != null)
            {
                throw new InternalDocumentException("internal", "document_exists");
            }

            var grantedToken = await _tokenStore.GetToken(authenticateParameter.WellKnownConfigurationUrl, authenticateParameter.ClientId, authenticateParameter.ClientSecret, new[] { "uma_protection" });
            if (grantedToken == null || string.IsNullOrWhiteSpace(grantedToken.AccessToken))
            {
                throw new InvalidConfigurationException("invalid_client_configuration");
            }

            var resource = await _identityServerUmaClientFactory.GetResourceSetClient().AddByResolution(new PostResourceSet
            {
                Name = $"officedocument_{document.Id}",
                Scopes = Constants.DEFAULT_SCOPES.ToList()
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (resource == null || string.IsNullOrWhiteSpace(resource.Id))
            {
                throw new InternalDocumentException("internal", "uma_resource_cannot_be_created");
            }

            var policy = await _identityServerUmaClientFactory.GetPolicyClient().AddByResolution(new PostPolicy
            {
                ResourceSetIds = new List<string> { resource.Id },
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
                        Scopes = Constants.DEFAULT_SCOPES.ToList()
                    }
                }
            }, authenticateParameter.WellKnownConfigurationUrl, grantedToken.AccessToken);
            if (policy == null || string.IsNullOrWhiteSpace(policy.PolicyId))
            {
                throw new InternalDocumentException("internal", "uma_policy_cannot_be_created");
            }

            officeDocument.UmaResourceId = resource.Id;
            officeDocument.UmaPolicyId = policy.PolicyId;
            if (!await _officeDocumentRepository.Add(officeDocument))
            {
                throw new InternalDocumentException("internal", "cannot_update_document");
            }

            return true;
        }

        /// <summary>
        /// Check the office document parameter.
        /// </summary>
        /// <param name="document"></param>
        private void Check(OfficeDocumentAggregate document)
        {
            if (string.IsNullOrWhiteSpace(document.Id))
            {
                throw new ArgumentNullException(nameof(document.Id));
            }

            if (string.IsNullOrWhiteSpace(document.Subject))
            {
                throw new ArgumentNullException(nameof(document.Subject));
            }

            if (string.IsNullOrWhiteSpace(document.PublicKey))
            {
                throw new ArgumentNullException(nameof(document.PublicKey));
            }            
            
            try
            {
                CheckXml(document.PublicKey);
            }
            catch(Exception)
            {
                throw new InternalDocumentException("internal", "invalid_public_key");
            }

            if (!string.IsNullOrWhiteSpace(document.PrivateKey))
            {
                try
                {
                    CheckXml(document.PrivateKey);
                }
                catch (Exception)
                {
                    throw new InternalDocumentException("internal", "invalid_private_key");
                }
            }
        }

        private static void CheckXml(string xml)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                using (var rsa = new RSACryptoServiceProvider())
                {
                    rsa.FromXmlStringCore(xml);
                }
            }
            else
            {
                using (var rsa = new RSAOpenSsl())
                {
                    rsa.FromXmlStringCore(xml);
                }
            }
        }
    }
}
