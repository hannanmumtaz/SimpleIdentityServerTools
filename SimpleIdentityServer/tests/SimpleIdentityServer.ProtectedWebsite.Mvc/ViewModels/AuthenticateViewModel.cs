using System.Runtime.Serialization;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc.ViewModels
{
    [DataContract]
    public class AuthenticateViewModel
    {
        [DataMember(Name = "login")]
        public string Login { get; set; }
        [DataMember(Name = "password")]
        public string Password { get; set; }
    }
}
