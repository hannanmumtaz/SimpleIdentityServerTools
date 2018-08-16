using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IOfficeDocumentClient
    {
        Task<BaseResponse> UpdateResolve(string documentId, UpdateOfficeDocumentRequest request, string configurationUrl, string accessToken);
        Task<BaseResponse> Update(string documentId, UpdateOfficeDocumentRequest request, string url, string accessToken);
        Task<GetOfficeDocumentResponse> GetResolve(string documentId, string configurationUrl, string accessToken);
        Task<GetOfficeDocumentResponse> Get(string documentId, string url, string accessToken);
        Task<BaseResponse> AddResolve(AddOfficeDocumentRequest request, string configurationUrl, string accessToken);
        Task<BaseResponse> Add(AddOfficeDocumentRequest request, string url, string accessToken);
        Task<GetDecryptedDocumentResponse> DecryptResolve(DecryptDocumentRequest request, string configurationUrl, string accessToken);
        Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken);
    }

    internal sealed class OfficeDocumentClient : IOfficeDocumentClient
    {
        private readonly IUpdateOfficeDocumentOperation _updateOfficeDocumentOperation;
        private readonly IGetOfficeDocumentOperation _getOfficeDocumentOperation;
        private readonly IAddOfficeDocumentOperation _addOfficeDocumentOperation;
        private readonly IDecryptOfficeDocumentOperation _decryptOfficeDocumentOperation;
        private readonly IGetConfigurationOperation _getConfigurationOperation;

        public OfficeDocumentClient(IUpdateOfficeDocumentOperation updateOfficeDocumentOperation, IGetOfficeDocumentOperation getOfficeDocumentOperation,
            IAddOfficeDocumentOperation addOfficeDocumentOperation, IDecryptOfficeDocumentOperation decryptOfficeDocumentOperation,
            IGetConfigurationOperation getConfigurationOperation)
        {
            _updateOfficeDocumentOperation = updateOfficeDocumentOperation;
            _getOfficeDocumentOperation = getOfficeDocumentOperation;
            _addOfficeDocumentOperation = addOfficeDocumentOperation;
            _decryptOfficeDocumentOperation = decryptOfficeDocumentOperation;
            _getConfigurationOperation = getConfigurationOperation;
        }

        public async Task<BaseResponse> UpdateResolve(string documentId, UpdateOfficeDocumentRequest request, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _updateOfficeDocumentOperation.Execute(documentId, request, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<BaseResponse> Update(string documentId, UpdateOfficeDocumentRequest request, string url, string accessToken)
        {
            return _updateOfficeDocumentOperation.Execute(documentId, request, $"{url}/officedocuments", accessToken);
        }

        public async Task<GetOfficeDocumentResponse> GetResolve(string documentId, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _getOfficeDocumentOperation.Execute(documentId, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetOfficeDocumentResponse> Get(string documentId, string url, string accessToken)
        {
            return _getOfficeDocumentOperation.Execute(documentId, url, accessToken);
        }

        public async Task<BaseResponse> AddResolve(AddOfficeDocumentRequest request, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _addOfficeDocumentOperation.Execute(request, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<BaseResponse> Add(AddOfficeDocumentRequest request, string url, string accessToken)
        {
            return _addOfficeDocumentOperation.Execute(request, url, accessToken);
        }

        public async Task<GetDecryptedDocumentResponse> DecryptResolve(DecryptDocumentRequest request, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _decryptOfficeDocumentOperation.Execute(request, configuration.OfficeDocumentsEndpoint + "/decrypt", accessToken).ConfigureAwait(false);
        }

        public Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken)
        {
            return _decryptOfficeDocumentOperation.Execute(request, url, accessToken);
        }
    }
}
