using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetOfficeDocumentResponse : BaseResponse
    {
        public string UmaResourceId { get; set; }
        public string UmaWellKnownUrl { get; set; }
        public OfficeDocumentResponse OfficeDocument { get; set; }
    }
}
