using System;

namespace SimpleIdentityServer.Module.Feed.Core.Exceptions
{
    public class BaseModuleFeedException : Exception
    {
        public BaseModuleFeedException() { }

        public BaseModuleFeedException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; set; }
    }
}