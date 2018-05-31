using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Loader
{
    public class ModuleLoaderOptions
    {
        public ModuleLoaderOptions()
        {
            NugetSources = new List<string>
            {
                "https://api.nuget.org/v3/index.json",
                "https://www.myget.org/F/advance-ict/api/v3/index.json"
            };
            NugetNbRetry = 5;
            NugetRetryAfterMs = 1000;
        }

        public int NugetNbRetry { get; set; }
        public int NugetRetryAfterMs { get; set; }
        public Uri ModuleFeedUri { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> NugetSources { get; set; }
    }
}
