using System;

namespace SimpleIdentityServer.Eid.UI.Helpers
{
    internal static class ConfigurationHelper
    {
        private const string _fakeEidEnabled = "FAKE_EID_ENABLED";

        public static bool IsFakeEidEnabled()
        {
            var envValue = Environment.GetEnvironmentVariable(_fakeEidEnabled);
            if (string.IsNullOrWhiteSpace(envValue))
            {
                return false;
            }

            bool result;
            if (!bool.TryParse(envValue, out result))
            {
                return false;
            }

            return result;
        }
    }
}
