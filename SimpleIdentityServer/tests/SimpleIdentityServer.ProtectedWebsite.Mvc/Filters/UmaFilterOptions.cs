namespace SimpleIdentityServer.ProtectedWebsite.Mvc.Filters
{
    public class UmaFilterOptions
    {
        public UmaFilterAuthorizationOptions Authorization { get; set; }
        public IIdentityTokenFetcher IdentityTokenFetcher { get; set; }
    }
}
