using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Client.Responses;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments
{
    public interface IOfficeDocumentClient
    {
        Task<GetOfficeDocumentResponse> GetResolve(string documentId, string configurationUrl, string accessToken);
        Task<GetOfficeDocumentResponse> Get(string documentId, string url, string accessToken);
        Task<BaseResponse> AddResolve(AddOfficeDocumentRequest request, string configurationUrl, string accessToken);
        Task<BaseResponse> Add(AddOfficeDocumentRequest request, string url, string accessToken);
        Task<GetDecryptedDocumentResponse> DecryptResolve(DecryptDocumentRequest request, string configurationUrl, string accessToken);
        Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken);
        Task<GetOfficeDocumentPermissionsResponse> GetPermissionsResolve(string documentId, string configurationUrl, string accessToken);
        Task<GetOfficeDocumentPermissionsResponse> GetPermissions(string documentId, string url, string accessToken);
        Task<GetInvitationLinkResponse> GetInvitationLinkResolve(string documentId, GenerateConfirmationCodeRequest request, string configurationUrl, string accessToken);
        Task<GetInvitationLinkResponse> GetInvitationLink(string documentId, GenerateConfirmationCodeRequest request, string url, string accessToken);
        Task<BaseResponse> ValidateInvitationLinkResolve(string confirmationCode, string configurationUrl, string accessToken);
        Task<BaseResponse> ValidateInvitationLink(string confirmationCode, string url, string accessToken);
        Task<GetAllInvitationLinksResponse> GetAllInvitationLinksResolve(string documentId, string configurationUrl, string accessToken);
        Task<GetAllInvitationLinksResponse> GetAllInvitationLinks(string documentId, string url, string accessToken);
        Task<BaseResponse> DeleteConfirmationLinkResolve(string confirmationCode, string configurationUrl, string accessToken);
        Task<BaseResponse> DeleteConfirmationLink(string confirmationCode, string url, string accessToken);
        Task<GetInvitationLinkInformationResponse> GetInvitationLinkInformationResolve(string confirmationCode, string configurationUrl, string accessToken);
        Task<GetInvitationLinkInformationResponse> GetInvitationLinkInformation(string confirmationCode, string url, string accessToken);
    }

    internal sealed class OfficeDocumentClient : IOfficeDocumentClient
    {
        private readonly IGetOfficeDocumentOperation _getOfficeDocumentOperation;
        private readonly IAddOfficeDocumentOperation _addOfficeDocumentOperation;
        private readonly IDecryptOfficeDocumentOperation _decryptOfficeDocumentOperation;
        private readonly IGetConfigurationOperation _getConfigurationOperation;
        private readonly IGetPermissionsOperation _getPermissionsOperation;
        private readonly IGetInvitationLinkOperation _getInvitationLinkOperation;
        private readonly IValidateConfirmationLinkOperation _validateConfirmationLinkOperation;
        private readonly IGetAllInvitationLinksOperation _getAllInvitationLinksOperation;
        private readonly IDeleteOfficeDocumentConfirmationCodeOperation _deleteOfficeDocumentConfirmationCodeOperation;
        private readonly IGetInvitationLinkInformationOperation _getInvitationLinkInformationOperation;

        public OfficeDocumentClient(IGetOfficeDocumentOperation getOfficeDocumentOperation,
            IAddOfficeDocumentOperation addOfficeDocumentOperation, IDecryptOfficeDocumentOperation decryptOfficeDocumentOperation,
            IGetConfigurationOperation getConfigurationOperation, IGetPermissionsOperation getPermissionsOperation,
            IGetInvitationLinkOperation getInvitationLinkOperation, IValidateConfirmationLinkOperation validateConfirmationLinkOperation,
            IGetAllInvitationLinksOperation getAllInvitationLinksOperation, IDeleteOfficeDocumentConfirmationCodeOperation deleteOfficeDocumentConfirmationCodeOperation,
            IGetInvitationLinkInformationOperation getInvitationLinkInformationOperation)
        {
            _getOfficeDocumentOperation = getOfficeDocumentOperation;
            _addOfficeDocumentOperation = addOfficeDocumentOperation;
            _decryptOfficeDocumentOperation = decryptOfficeDocumentOperation;
            _getConfigurationOperation = getConfigurationOperation;
            _getPermissionsOperation = getPermissionsOperation;
            _getInvitationLinkOperation = getInvitationLinkOperation;
            _validateConfirmationLinkOperation = validateConfirmationLinkOperation;
            _getAllInvitationLinksOperation = getAllInvitationLinksOperation;
            _deleteOfficeDocumentConfirmationCodeOperation = deleteOfficeDocumentConfirmationCodeOperation;
            _getInvitationLinkInformationOperation = getInvitationLinkInformationOperation;
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
            return await _decryptOfficeDocumentOperation.Execute(request, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetDecryptedDocumentResponse> Decrypt(DecryptDocumentRequest request, string url, string accessToken)
        {
            return _decryptOfficeDocumentOperation.Execute(request, url, accessToken);
        }

        public async Task<GetOfficeDocumentPermissionsResponse> GetPermissionsResolve(string documentId, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _getPermissionsOperation.Execute(documentId, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetOfficeDocumentPermissionsResponse> GetPermissions(string documentId, string url, string accessToken)
        {
            return _getPermissionsOperation.Execute(documentId, url, accessToken);
        }

        public async Task<GetInvitationLinkResponse> GetInvitationLinkResolve(string documentId, GenerateConfirmationCodeRequest request, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _getInvitationLinkOperation.Execute(documentId, request, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetInvitationLinkResponse> GetInvitationLink(string documentId, GenerateConfirmationCodeRequest request, string url, string accessToken)
        {
            return _getInvitationLinkOperation.Execute(documentId, request, url, accessToken);
        }

        public async Task<BaseResponse> ValidateInvitationLinkResolve(string confirmationCode, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _validateConfirmationLinkOperation.Execute(confirmationCode, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<BaseResponse> ValidateInvitationLink(string confirmationCode, string url, string accessToken)
        {
            return _validateConfirmationLinkOperation.Execute(confirmationCode, url, accessToken);
        }

        public async Task<GetAllInvitationLinksResponse> GetAllInvitationLinksResolve(string documentId, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _getAllInvitationLinksOperation.Execute(documentId, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetAllInvitationLinksResponse> GetAllInvitationLinks(string documentId, string url, string accessToken)
        {
            return _getAllInvitationLinksOperation.Execute(documentId, url, accessToken);
        }

        public async Task<BaseResponse> DeleteConfirmationLinkResolve(string confirmationCode, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _deleteOfficeDocumentConfirmationCodeOperation.Execute(confirmationCode, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<BaseResponse> DeleteConfirmationLink(string confirmationCode, string url, string accessToken)
        {
            return _deleteOfficeDocumentConfirmationCodeOperation.Execute(confirmationCode, url, accessToken);
        }

        public async Task<GetInvitationLinkInformationResponse> GetInvitationLinkInformationResolve(string confirmationCode, string configurationUrl, string accessToken)
        {
            var configuration = await _getConfigurationOperation.Execute(new Uri(configurationUrl)).ConfigureAwait(false);
            return await _getInvitationLinkInformationOperation.Execute(confirmationCode, configuration.OfficeDocumentsEndpoint, accessToken).ConfigureAwait(false);
        }

        public Task<GetInvitationLinkInformationResponse> GetInvitationLinkInformation(string confirmationCode, string url, string accessToken)
        {
            return _getInvitationLinkInformationOperation.Execute(confirmationCode, url, accessToken);
        }
    }
}
