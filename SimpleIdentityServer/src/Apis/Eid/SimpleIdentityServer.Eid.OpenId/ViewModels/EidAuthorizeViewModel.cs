using SimpleIdentityServer.Host.ViewModels;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Eid.OpenId.ViewModels
{
    [DataContract]
    public class EidAuthorizeViewModel
    {
        [DataMember(Name = Constants.EidAuthorizeViewModelNames.Code)]
        public string Code { get; set; }
        [DataMember(Name = Constants.EidAuthorizeViewModelNames.Xml)]
        public string Xml { get; set; }
        [DataMember(Name = Constants.EidAuthorizeViewModelNames.IdProviders)]
        public IEnumerable<IdProviderViewModel> IdProviders { get; set; }
    }
}
