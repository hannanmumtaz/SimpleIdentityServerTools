using SimpleIdentityServer.Authenticate.Basic.ViewModels;

namespace SimpleIdentityServer.Authenticate.Eid.ViewModels
{
    public class LoginViewModel : AuthorizeViewModel
    {
        public string Xml { get; set; }
    }
}
