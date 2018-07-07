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

            return new ModuleLoader(options);
        }
    }
}
