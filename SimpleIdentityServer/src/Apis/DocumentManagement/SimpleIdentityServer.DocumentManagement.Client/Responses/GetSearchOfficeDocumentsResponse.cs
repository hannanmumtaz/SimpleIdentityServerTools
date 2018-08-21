using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetSearchOfficeDocumentsResponse : BaseResponse
    {
        public SearchOfficeDocumentsResponse Content { get; set; }
    }
}
