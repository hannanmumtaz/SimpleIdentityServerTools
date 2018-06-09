namespace SimpleIdentityServer.DocumentManagement.Api.Extensions
{
    public class OAuthOptions
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WellKnownConfiguration { get; set; }
    }

    public class DocumentManagementApiOptions
    {
        public DocumentManagementApiOptions(string openIdWellKnownConfiguration)
        {
            OAuth = new OAuthOptions();
            OpenIdWellKnownConfiguration = openIdWellKnownConfiguration;
        }

        public OAuthOptions OAuth { get; set; }
        public string OpenIdWellKnownConfiguration { get; private set; }
    }
}
