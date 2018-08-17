using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.DocumentManagement.Client.Responses
{
    public class GetOfficeDocumentPermissionsResponse : BaseResponse
    {
        public IEnumerable<OfficeDocumentPermissionResponse> Content { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaWellKnownUrl { get; set; }
    }
}
