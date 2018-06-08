using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt.Signature;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Core.Repositories;
using System;
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

            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
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

            var result = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(authenticateParameter.ClientId, authenticateParameter.ClientSecret).Introspect(accessToken, TokenType.AccessToken)
                .ResolveAsync(authenticateParameter.WellKnownConfigurationUrl);
            if (result == null || !result.Active)
            {
                throw new InternalDocumentException("parameter", "not_valid_accesstoken");
            }

            var payload = _jwsParser.GetPayload(accessToken);
            var ticketId = string.Empty; // RETRIEVE THE TICKET
            return officeDocument;
        }
    }
}
