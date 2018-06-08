using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class OfficeDocumentPermissionRequest
    {
        [DataMember(Name = "sub")]
        public string Subject { get; set; }
        [DataMember(Name = "scopes")]
        public IEnumerable<string> Scopes { get; set; }
    }

    [DataContract]
    public class UpdateOfficeDocumentRequest
    {
        [DataMember(Name = "permissions")]
        public IEnumerable<OfficeDocumentPermissionRequest> Permissions { get; set; }
    }
}
