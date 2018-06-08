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
        public DocumentManagementApiOptions()
        {
            OAuth = new OAuthOptions();
        }

        public OAuthOptions OAuth { get; set; }
    }
}
