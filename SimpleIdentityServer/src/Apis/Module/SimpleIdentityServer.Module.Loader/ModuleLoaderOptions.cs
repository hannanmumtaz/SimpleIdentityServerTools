﻿using System;
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
        }

        public Uri ModuleFeedUri { get; set; }
        public string ProjectName { get; set; }
        public string Version { get; set; }
        public string ModulePath { get; set; }
        public IEnumerable<string> NugetSources { get; set; }
    }
}
