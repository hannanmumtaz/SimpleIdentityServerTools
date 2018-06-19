using NuGet.Server.Core.Infrastructure;
using NuGet.Server.V2.Controllers;

namespace SimpleIdentityServer.Nuget.Feed.Controllers
{
    public class NuGetPublicODataController : NuGetODataController
    {
        public NuGetPublicODataController()
            : base(Program.NuGetPublicRepository, new ApiKeyPackageAuthenticationService(true, Program.ApiKey))
        {

        }
    }
}