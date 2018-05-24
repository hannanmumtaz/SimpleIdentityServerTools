namespace SimpleIdentityServer.ResourceManager.API.Host.Extensions
{
    public class ResourceManagerHostOptions
    {
        public string AuthWellKnownConfiguration { get; set; }
        public string AuthClientId { get; set;}
        public string AuthClientSecret { get; set; }
    }
}
