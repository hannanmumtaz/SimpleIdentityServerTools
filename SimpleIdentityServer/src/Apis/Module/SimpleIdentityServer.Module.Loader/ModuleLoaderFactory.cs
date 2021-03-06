﻿using SimpleIdentityServer.Module.Feed.Client;
using SimpleIdentityServer.Module.Loader.Factories;
using SimpleIdentityServer.Module.Loader.Nuget;
using System;

namespace SimpleIdentityServer.Module.Loader
{
    public class ModuleLoaderFactory
    {
        public IModuleLoader BuidlerModuleLoader(ModuleLoaderOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var nugetClient = new NugetClient(new HttpClientFactory());
            var moduleFeedClientFactory = new ModuleFeedClientFactory();
            return new ModuleLoader(nugetClient, moduleFeedClientFactory, options);
        }
    }
}
