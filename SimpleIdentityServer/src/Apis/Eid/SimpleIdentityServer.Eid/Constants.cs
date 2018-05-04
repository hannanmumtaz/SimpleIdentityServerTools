namespace SimpleIdentityServer.Eid
{
    internal class Constants
    {
        public static class ErrorCodes
        {
            public const string SelectedFileNotActivated = "selected_file_not_activated";
            public const string NoPreciseDiagnostic = "no_precise_diagnostic";
            public const string EEPromCorrupted = "eeprom_corrupted";
            public const string FileNotFound = "file_not_found";
            public const string WrongParameterP1P2 = "wrong_parameter_p1_p2";
            public const string LcInconsistentWithP1P2 = "lc_inconsistent_with_p1_p2";
            public const string CommandNotAvailable = "command_not_available";
            public const string ClaNotSupported = "cla_not_supported";
            public const string CannotBeginTransaction = "cannot_be_transaction";
            public const string NoEstablishedConnection = "no_established_connection";
            public const string NoEstablishedContext = "no_established_context";
            public const string ConnectionExists = "connection_exists";
            public const string ContextExists = "context_exists";
        }
    }
}
