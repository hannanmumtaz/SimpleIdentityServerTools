using SimpleIdentityServer.Authenticate.Basic;

namespace SimpleIdentityServer.Authenticate.Eid
{
    public class EidAuthenticateOptions : BasicAuthenticateOptions
    {
        public EidAuthenticateOptions()
        {
            ImagePath = "img";
        }

        public string EidUrl { get; set; }
        public string ImagePath { get; set; }
    }
}
