﻿using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments
{
    public interface IOfficeDocumentActions
    {
        Task<bool> Add(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter);
        Task<OfficeDocumentAggregate> Get(string documentId, string accessToken, AuthenticateParameter authenticateParameter);
        Task<bool> Update(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter);
        Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter);
    }

    internal sealed class OfficeDocumentActions : IOfficeDocumentActions
    {
        private readonly IAddOfficeDocumentAction _addOfficeDocumentAction;
        private readonly IGetOfficeDocumentAction _getOfficeDocumentAction;
        private readonly IUpdateOfficeDocumentAction _updateOfficeDocumentAction;
        private readonly IDecryptOfficeDocumentAction _decryptOfficeDocumentAction;

        public OfficeDocumentActions(IAddOfficeDocumentAction addOfficeDocumentAction, IGetOfficeDocumentAction getOfficeDocumentAction, 
            IUpdateOfficeDocumentAction updateOfficeDocumentAction, IDecryptOfficeDocumentAction decryptOfficeDocumentAction)
        {
            _addOfficeDocumentAction = addOfficeDocumentAction;
            _getOfficeDocumentAction = getOfficeDocumentAction;
            _updateOfficeDocumentAction = updateOfficeDocumentAction;
            _decryptOfficeDocumentAction = decryptOfficeDocumentAction;
        }

        public Task<bool> Add(string openidWellKnownConfiguration, AddDocumentParameter document, AuthenticateParameter authenticateParameter)
        {
            return _addOfficeDocumentAction.Execute(openidWellKnownConfiguration, document, authenticateParameter);
        }

        public Task<OfficeDocumentAggregate> Get(string documentId, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _getOfficeDocumentAction.Execute(documentId, accessToken, authenticateParameter);
        }

        public Task<bool> Update(string wellKnownConfiguration, string documentId, UpdateOfficeDocumentParameter parameter, AuthenticateParameter authenticateParameter)
        {
            return _updateOfficeDocumentAction.Execute(wellKnownConfiguration, documentId, parameter, authenticateParameter);
        }

        public Task<DecryptedResponse> Decrypt(DecryptOfficeDocumentParameter decryptOfficeDocumentParameter, string accessToken, AuthenticateParameter authenticateParameter)
        {
            return _decryptOfficeDocumentAction.Execute(decryptOfficeDocumentParameter, accessToken, authenticateParameter);
        }
    }
}
