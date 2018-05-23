namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Filters
{
    public class UmaFilterAuthorizationOptions
    {
        public string AuthorizationWellKnownConfiguration { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
