namespace SimpleIdentityServer.Uma.Authentication
{
    public class UmaFilterOptions
    {
        public UmaFilterOptions()
        {
            Authorization = new UmaFilterAuthorizationOptions();
            Cookie = new UmaFilterCookieOptions();
            IdentityTokenFetcher = new IdTokenCookieFetcher();
            ResourceManager = new UmaFilterResourceManagerAuthorizationOptions();
        }

        public UmaFilterAuthorizationOptions Authorization { get; set; }
        public UmaFilterCookieOptions Cookie { get; set; }
        public UmaFilterResourceManagerAuthorizationOptions ResourceManager { get; set; }
        public IIdentityTokenFetcher IdentityTokenFetcher { get; set; }
    }
}
