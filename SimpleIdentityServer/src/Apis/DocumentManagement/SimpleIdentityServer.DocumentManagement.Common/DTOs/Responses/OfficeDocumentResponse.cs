using System;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    public enum OfficeDocumentEncAlgorithms
    {
        [EnumMember(Value = "rsa")]
        RSA
    }

    [DataContract]
    public class OfficeDocumentResponse
    {
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "enc_alg")]
        public OfficeDocumentEncAlgorithms? EncAlg { get; set; }
        [DataMember(Name = "enc_password")]
        public string EncPassword { get; set; }
        [DataMember(Name = "enc_salt")]
        public string EncSalt { get; set; }
        [DataMember(Name = "create_datetime")]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = "update_datetime")]
        public DateTime UpdateDateTime { get; set; }
    }
}
