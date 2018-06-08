namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class AuthenticateParameter
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string WellKnownConfigurationUrl { get; set; }
    }
}
