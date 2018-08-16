using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGetOfficeDocumentAction
    {
        Task<OfficeDocumentAggregate> Execute(string documentId, string accessToken, AuthenticateParameter authenticateParameter);
    }

    internal sealed class GetOfficeDocumentAction : IGetOfficeDocumentAction
    {
        private readonly IOfficeDocumentRepository _officeDocumentRepository;
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IJwsParser _jwsParser;

        public GetOfficeDocumentAction(IOfficeDocumentRepository officeDocumentRepository, IIdentityServerClientFactory identityServerClientFactory, IJwsParser jwsParser)
        {
            _officeDocumentRepository = officeDocumentRepository;
            _identityServerClientFactory = identityServerClientFactory;
            _jwsParser = jwsParser;
        }

        public async Task<OfficeDocumentAggregate> Execute(string documentId, string accessToken, AuthenticateParameter authenticateParameter)
        {
            if (string.IsNullOrWhiteSpace(documentId))
            {
                throw new ArgumentNullException(nameof(documentId));
            }

            if (authenticateParameter == null)
            {
                throw new ArgumentNullException(nameof(authenticateParameter));
            }

            var officeDocument = await _officeDocumentRepository.Get(documentId);
            if (officeDocument == null)
            {
                throw new DocumentNotFoundException();
            }

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new NoUmaAccessTokenException(officeDocument.UmaResourceId, authenticateParameter.WellKnownConfigurationUrl);
            }

            var result = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(authenticateParameter.ClientId, authenticateParameter.ClientSecret).Introspect(accessToken, TokenType.AccessToken)
                .ResolveAsync(authenticateParameter.WellKnownConfigurationUrl);
            if (result.ContainsError || !result.Content.Active)
            {
                throw new NotAuthorizedException(ErrorCodes.InvalidRequest, ErrorDescriptions.AccessTokenIsNotValid);
            }

            var payload = _jwsParser.GetPayload(accessToken);
            if (!payload.ContainsKey("ticket"))
            {
                throw new NotAuthorizedException(ErrorCodes.Authorization, ErrorDescriptions.NoTicket);
            }

            var tickets = payload["ticket"] as JArray;
            if (tickets == null)
            {
                throw new NotAuthorizedException(ErrorCodes.Authorization, ErrorDescriptions.NoTicket);
            }

            var accessibleScopes = new List<string>();
            foreach(var jObj in tickets)
            {
                if (jObj["resource_id"].ToString() == officeDocument.UmaResourceId)
                {
                    accessibleScopes = jObj["scopes"].ToString().Split(' ').ToList();
                    break;
                }
            }

            accessibleScopes = accessibleScopes.Where(s => Constants.DEFAULT_SCOPES.Contains(s)).ToList();
            if (!accessibleScopes.Any())
            {
                throw new NotAuthorizedException(ErrorCodes.Authorization, ErrorDescriptions.NotAuthorized);
            }
            
            return officeDocument;
        }
    }
}
