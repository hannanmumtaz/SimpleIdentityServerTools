using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetInvitationLinkInformationResponse : BaseResponse
    {
        public OfficeDocumentInvitationLinkResponse Content { get; set; }
    }
}
