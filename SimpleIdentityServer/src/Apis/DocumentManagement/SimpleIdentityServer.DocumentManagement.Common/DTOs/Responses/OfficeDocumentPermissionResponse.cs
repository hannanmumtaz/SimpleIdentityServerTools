using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class OfficeDocumentPermissionResponse
    {
        [DataMember(Name = "sub")]
        public string UserSubject { get; set; }
    }
}
