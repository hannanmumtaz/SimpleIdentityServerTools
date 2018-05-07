namespace RpEid.Api
{
    public static class Constants
    {
        public static class AccountNames
        {
            public const string Subject = "subject";
            public const string Name = "name";
            public const string Email = "email";
            public const string IsConfirmed = "is_confirmed";
            public const string CreateDateTime = "create_datetime";
            public const string UpdateDateTime = "update_datetime";
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
    }
}
