using System.Runtime.Serialization;

namespace SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses
{
    [DataContract]
    public class DecryptedResponse
    {
        [DataMember(Name = "password")]
        public string Password { get; set; }
        [DataMember(Name = "salt")]
        public string Salt { get; set; }
    }
}
