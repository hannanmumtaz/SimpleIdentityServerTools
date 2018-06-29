namespace SimpleIdentityServer.ResourceManager.Resolver
{
    public class ResourceManagerResolverOptions
    {
        public string Url { get; set; }
        public string ResourceManagerUrl { get; set; }
        public ResourceManagerResolverAuthorizationOptions Authorization { get; set; }
    }
}
