using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetInvitationLinkResponse : BaseResponse
    {
        public OfficeDocumentConfirmationLinkResponse Content { get; set; }
    }
}
