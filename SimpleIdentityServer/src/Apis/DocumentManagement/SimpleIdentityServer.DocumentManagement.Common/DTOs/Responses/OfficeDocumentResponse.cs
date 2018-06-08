using System;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class OfficeDocumentResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "public_key")]
        public string PublicKey { get; set; }
        [DataMember(Name = "private_key")]
        public string PrivateKey { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
