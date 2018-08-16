using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class DecryptDocumentRequest
    {
        [DataMember(Name = "document_id")]
        public string DocumentId { get; set; }
        [DataMember(Name = "kid")]
        public string Kid { get; set; }
        [DataMember(Name = "credentials")]
        public string Credentials { get; set; }
    }
}
