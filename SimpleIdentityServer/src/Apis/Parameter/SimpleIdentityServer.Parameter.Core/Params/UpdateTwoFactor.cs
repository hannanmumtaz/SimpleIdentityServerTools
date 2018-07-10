using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Params
{
    public class UpdateTwoFactor
    {
        public string Name { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
    }
}
