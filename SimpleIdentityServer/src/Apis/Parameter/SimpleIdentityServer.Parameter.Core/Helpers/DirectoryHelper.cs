using System.IO;

namespace SimpleIdentityServer.Parameter.Core.Helpers
{
    public interface IDirectoryHelper
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
