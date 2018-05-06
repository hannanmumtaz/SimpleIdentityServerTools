using SimpleIdentityServer.Host.ViewModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Eid.OpenId.ViewModels
{
    [DataContract]
    public class LoginViewModel
    {
        [DataMember(Name = Constants.LoginViewModelNames.IdProviders)]
        public IEnumerable<IdProviderViewModel> IdProviders { get; set; }
        [DataMember(Name = Constants.LoginViewModelNames.Xml)]
        public string Xml { get; set; }
    }
}
