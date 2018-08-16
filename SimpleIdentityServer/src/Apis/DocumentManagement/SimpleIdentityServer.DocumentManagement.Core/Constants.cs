using System.Collections.Generic;

namespace SimpleIdentityServer.DocumentManagement.Core
{
    internal static class Constants
    {
        public static IEnumerable<string> DEFAULT_SCOPES = new[]
        {
            "read",
            "print"
        };
    }
}
