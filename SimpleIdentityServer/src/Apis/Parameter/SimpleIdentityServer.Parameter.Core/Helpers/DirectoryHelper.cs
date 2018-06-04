using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Helpers
{
    internal interface IDirectoryHelper
    {
        string GetCurrentDirectory();
    }

    internal sealed class DirectoryHelper : IDirectoryHelper
    {
        public string GetCurrentDirectory()
        {
            return Directory.GetCurrentDirectory();
        }
    }
}
