namespace SimpleIdentityServer.Connectors.Facebook
{
    public static class FacebookDefaults
    {
        public const string AuthenticationScheme = "Facebook";
        public static readonly string DisplayName = "Facebook";
        public static readonly string AuthorizationEndpoint = "https://www.facebook.com/v2.12/dialog/oauth";
        public static readonly string TokenEndpoint = "https://graph.facebook.com/v2.12/oauth/access_token";
        public static readonly string UserInformationEndpoint = "https://graph.facebook.com/v2.12/me";
    }
}