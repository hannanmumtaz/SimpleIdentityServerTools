using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetOfficeDocumentResponse : BaseResponse
    {
        public OfficeDocumentResponse OfficeDocument { get; set; }
    }
}
