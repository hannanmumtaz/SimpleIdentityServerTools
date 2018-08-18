using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests
{
    [DataContract]
    public class GenerateConfirmationCodeRequest
    {
        [DataMember(Name = "exp")]
        public int? ExpiresIn { get; set; }
        [DataMember(Name = "nb")]
        public int? NumberOfConfirmations { get; set; }
    }
}
