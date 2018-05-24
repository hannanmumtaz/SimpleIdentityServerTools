namespace SimpleIdentityServer.ResourceManager.Resolver
{
    public class ResourceManagerResolverAuthorizationOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthorizationWellKnownConfiguration { get; set; }
    }
}
