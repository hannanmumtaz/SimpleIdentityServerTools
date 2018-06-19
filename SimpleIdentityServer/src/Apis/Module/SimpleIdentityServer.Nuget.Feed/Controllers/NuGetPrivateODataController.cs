using NuGet.Server.V2.Controllers;
using System.Web.Http;

namespace SimpleIdentityServer.Nuget.Feed.Controllers
{
    [Authorize]
    public class NuGetPrivateODataController : NuGetODataController
    {
        public NuGetPrivateODataController()
            : base(Program.NuGetPrivateRepository)
        {
        }
    }
}