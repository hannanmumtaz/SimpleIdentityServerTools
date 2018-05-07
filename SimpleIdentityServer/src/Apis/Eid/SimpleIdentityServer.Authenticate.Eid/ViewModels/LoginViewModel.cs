using SimpleIdentityServer.Host.ViewModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Authenticate.Eid.ViewModels
{
    [DataContract]
    public class LoginViewModel
    {
        [DataMember(Name = Constants.LoginViewModelNames.IdProviders)]
        public IEnumerable<IdProviderViewModel> IdProviders { get; set; }
        [DataMember(Name = Constants.LoginViewModelNames.Xml)]
        public string Xml { get; set; }
        [DataMember(Name = Constants.LoginViewModelNames.EidUrl)]
        public string EidUrl { get; set; }
    }
}
