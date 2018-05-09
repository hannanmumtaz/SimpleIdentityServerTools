namespace RpEid.Api
{
    public static class Constants
    {
        public static class AccountNames
        {
            public const string Subject = "subject";
            public const string Name = "name";
            public const string Email = "email";
            public const string IsGranted = "is_granted";
            public const string IsConfirmed = "is_confirmed";
            public const string CreateDateTime = "create_datetime";
            public const string UpdateDateTime = "update_datetime";
            public const string GrantDateTime = "grant_datetime";
            public const string ConfirmationDateTime = "confirmation_datetime";
        }

        public static class ErrorResponseNames
        {
            public const string Code = "error";
            public const string Message = "error_description";
        }

        public static class ErrorCodes
        {
            public const string IncompleteRequest = "incomplete_request";
            public const string InvalidConfiguration = "invalid_configuration";
            public const string InvalidRequest = "invalid_request";
        }

        public static class SearchNames
        {
            public const string StartIndex = "start_index";
            public const string Count = "count";
            public const string Order = "order"; 
        }

        public static class SearchAccountsNames
        {
            public const string Subjects = "subjects";
            public const string IsConfirmed = "is_confirmed";
        }

        public static class RouteNames
        {
            public const string Accounts = "accounts";
        }
    }
}
