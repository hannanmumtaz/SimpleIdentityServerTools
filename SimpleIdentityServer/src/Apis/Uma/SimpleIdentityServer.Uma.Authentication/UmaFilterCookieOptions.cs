namespace SimpleIdentityServer.Uma.Authentication
{
    public class UmaFilterCookieOptions
    {
        public UmaFilterCookieOptions()
        {
            CookieName = Constants.DEFAULT_COOKIE_NAME;
            ExpiresIn = Constants.DEFAULT_EXPIRES_IN;
        }

        public UmaFilterCookieOptions(string cookieName, double expiresIn = Constants.DEFAULT_EXPIRES_IN)
        {
            CookieName = cookieName;
            ExpiresIn = expiresIn;
        }

        public string CookieName { get; set; }
        public double ExpiresIn { get; set; }
    }
}
