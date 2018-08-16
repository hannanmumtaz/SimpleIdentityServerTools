using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IOfficeDocumentClient
    {
        Task<BaseResponse> Update(string documentId, UpdateOfficeDocumentRequest request, string url, string accessToken);
        Task<GetOfficeDocumentResponse> Get(string documentId, string url, string accessToken);
        Task<BaseResponse> Add(AddOfficeDocumentRequest request, string url, string accessToken);
        Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken);
    }

    internal sealed class OfficeDocumentClient : IOfficeDocumentClient
    {
        private readonly IUpdateOfficeDocumentOperation _updateOfficeDocumentOperation;
        private readonly IGetOfficeDocumentOperation _getOfficeDocumentOperation;
        private readonly IAddOfficeDocumentOperation _addOfficeDocumentOperation;
        private readonly IDecryptOfficeDocumentOperation _decryptOfficeDocumentOperation;

        public OfficeDocumentClient(IUpdateOfficeDocumentOperation updateOfficeDocumentOperation, IGetOfficeDocumentOperation getOfficeDocumentOperation,
            IAddOfficeDocumentOperation addOfficeDocumentOperation, IDecryptOfficeDocumentOperation decryptOfficeDocumentOperation)
        {
            _updateOfficeDocumentOperation = updateOfficeDocumentOperation;
            _getOfficeDocumentOperation = getOfficeDocumentOperation;
            _addOfficeDocumentOperation = addOfficeDocumentOperation;
            _decryptOfficeDocumentOperation = decryptOfficeDocumentOperation;
        }

        public Task<BaseResponse> Update(string documentId, UpdateOfficeDocumentRequest request, string url, string accessToken)
        {
            return _updateOfficeDocumentOperation.Execute(documentId, request, $"{url}/officedocuments", accessToken);
        }

        public Task<GetOfficeDocumentResponse> Get(string documentId, string url, string accessToken)
        {
            return _getOfficeDocumentOperation.Execute(documentId, $"{url}/officedocuments", accessToken);
        }

        public Task<BaseResponse> Add(AddOfficeDocumentRequest request, string url, string accessToken)
        {
            return _addOfficeDocumentOperation.Execute(request, $"{url}/officedocuments", accessToken);
        }

        public Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken)
        {
            return _decryptOfficeDocumentOperation.Execute(request, $"{url}/officedocuments/decrypt", accessToken);
        }
    }
}
