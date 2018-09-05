namespace SimpleIdentityServer.Authenticate.Eid
{
    internal static class Constants
    {
        public const string AMR = "eid";

        public static class LoginViewModelNames
        {
            public const string Xml = "xml";
            public const string IdProviders = "id_providers";
            public const string EidUrl = "eid_url";
        }

        public static class EidAuthorizeViewModelNames
        {
            public const string Code = "code";
            public const string Xml = "xml";
            public const string IdProviders = "id_providers";
            public const string EidUrl = "eid_url";
        }
    }
}
