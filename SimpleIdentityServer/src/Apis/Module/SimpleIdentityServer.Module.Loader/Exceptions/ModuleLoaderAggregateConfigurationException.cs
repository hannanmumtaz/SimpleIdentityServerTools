using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Loader.Exceptions
{
    public class ModuleLoaderAggregateConfigurationException : Exception
    {
        public ModuleLoaderAggregateConfigurationException(string message, IEnumerable<string> messages) : base(message)
        {
            Messages = messages;
        }

        public IEnumerable<string> Messages { get; private set; }
    }
}
