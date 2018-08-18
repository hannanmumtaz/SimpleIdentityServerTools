using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetAllInvitationLinksResponse : BaseResponse
    {
        public IEnumerable<OfficeDocumentInvitationLinkResponse> Content { get; set; }
    }
}
