namespace SimpleIdentityServer.Eid.UI
{
    internal static class Constants
    {
        public static class ErrorCodes
        {
            public const string Server = "server";
            public const string Request = "request";
            public const string Eid = "eid";
        }

        public static class ErrorMessages
        {
            public const string CardError = "card_error";
            public const string NoCard = "no_card";
            public const string NoPinCode = "no_pincode";
            public const string ActiveSession = "active_session";
            public const string NoType = "no_type";
            public const string InvalidSession = "invalid_session";
        }

        public static class DtoPropertyNames
        {
            public const string PinCode = "pin_code";
            public const string Type = "type";
        }

        public static class ActionNames
        {
            public const string Session = "session";
        }
    }
}
