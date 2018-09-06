using SimpleIdentityServer.Authenticate.Basic.ViewModels;

namespace SimpleIdentityServer.Authenticate.Eid.ViewModels
{
    public class EidAuthorizeViewModel : AuthorizeOpenIdViewModel
    {
        public string Xml { get; set; }
    }
}
