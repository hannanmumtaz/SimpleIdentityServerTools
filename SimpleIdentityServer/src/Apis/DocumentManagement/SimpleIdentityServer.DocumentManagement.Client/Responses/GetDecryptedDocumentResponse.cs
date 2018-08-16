using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetDecryptedDocumentResponse : BaseResponse
    {
        public DecryptedResponse Content { get; set; }
    }
}
