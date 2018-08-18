namespace SimpleIdentityServer.DocumentManagement.Core
{
    public static class ErrorDescriptions
    {
        public const string AccessTokenIsNotValid = "access token is not valid";
        public const string NoTicket = "no ticket";
        public const string TheConfirmationCodeIsNotValid = "the confirmation code is not valid";
        public const string ConfirmationCodeIsExpired = "the confirmation code has expired";
        public const string NotEnoughConfirmationCode = "there is not enough confirmation code";
        public const string NoUmaResource = "no uma resource";
        public const string NoUmaPolicy = "no uma policy";
        public const string SubjectIsMissing = "the subject is missing";
        public const string ParameterIsMissing = "parameter '{0}' is missing";
        public const string OfficeDocumentExists = "office document already exists";
        public const string NotAuthorized = "not authorized";
        public const string ScopesAreNotValid = "at least one scope is not valid";
        public const string SubjectIsMandatoryInThePermission = "the subject is mandatory in the permission";
        public const string NoRequest = "no request";
        public const string TheJsonWebKeyDoesntExist = "the json web key {0} doesn't exist";
        public const string UmaPolicyDoesntExist = "the uma policy doesn't exist";
        public const string UmaPolicyCannotBeUpdated = "the uma policy cannot be updated";
        public const string CannotRetrieveAccessToken = "an error occured while trying to get an access token";
        public const string CannotAddUmaResource = "an error occured while trying to add the UMA resource";
        public const string CannotAddUmaPolicy = "an error occured while trying to add the UMA policy";
        public const string CannotGetUmaPolicy = "an error occured while trying to get the UMA policy";
        public const string CannotAddOfficeDocument = "an error occured while trying to add the office document";
    }
}
