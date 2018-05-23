namespace SimpleIdentityServer.Uma.Authentication
{
    public class UmaFilterOptions
    {
        public UmaFilterAuthorizationOptions Authorization { get; set; }
        public IIdentityTokenFetcher IdentityTokenFetcher { get; set; }
    }
}
