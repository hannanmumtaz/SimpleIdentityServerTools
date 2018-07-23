namespace SimpleIdentityServer.Authenticate.Eid
{
    public class EidAuthenticateOptions
    {
        public EidAuthenticateOptions()
        {
            ImagePath = "img";
        }

        public string EidUrl { get; set; }
        public string ImagePath { get; set; }
    }
}
