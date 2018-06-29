namespace SimpleIdentityServer.Profile.Host
{
    internal static class Constants
    {
        public static class RouteNames
        {
            public const string ProfilesController = "profiles";
	    public const string EndpointsController = "endpoints";
        }

        public static class ProfileResponseNames
        {
            public const string AuthUrl = "auth_url";
            public const string OpenidUrl = "openid_url";
            public const string ScimUrl = "scim_url";
        }

        public static class EndpointNames
        {
            public const string Url = "url";
            public const string Name = "name";
            public const string Description = "description";
            public const string Type = "type";
            public const string CreateDateTime = "create_datetime";
        }

        public static class ElFinderIdProviderResponseNames
        {
            public const string Url = "url";
            public const string Name = "name";
            public const string Description = "description";
        }

        public static class ErrorDtoNames
        {
            public const string Error = "error";
            public const string Code = "code";
            public const string Message = "message";
        }

        public static class ErrorCodes
        {
            public const string InternalError = "internal_error";
        }

        public static class Errors
        {
            public const string ErrParamNotSpecified = "the parameter {0} is not specified";
            public const string ErrRemoveEndpoint = "an error occured while trying to remove the endpoint(s)";
            public const string ErrUpdateProfile = "an error occured while trying to update the profile";
        }
    }
}
