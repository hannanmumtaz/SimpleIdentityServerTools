namespace SimpleIdentityServer.Module.Feed.Core.Exceptions
{
    public class ModuleFeedInternalException : BaseModuleFeedException
    {
        public ModuleFeedInternalException(string code, string message) : base(code, message) { }
    }
}
