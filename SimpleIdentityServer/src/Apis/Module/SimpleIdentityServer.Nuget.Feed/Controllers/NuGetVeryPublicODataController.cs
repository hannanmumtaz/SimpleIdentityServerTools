using NuGet.Server.Core.Infrastructure;
using NuGet.Server.V2.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimpleIdentityServer.Nuget.Feed.Controllers
{
    public class NuGetVeryPublicODataController : NuGetODataController
    {
        static IPackageAuthenticationService CreateAuthenticationService()
        {
            return new ApiKeyPackageAuthenticationService(false, null);
        }

        public NuGetVeryPublicODataController()
            : base(Program.NuGetVeryPublicRepository, CreateAuthenticationService())
        {

        }

        //Uncomment lines below to only allow delete for authorized users in Admin role
        //[Authorize(Roles = "Admin")]
        //public override Task<HttpResponseMessage> UploadPackage()
        //{
        //    return base.UploadPackage();
        //}

        //Uncomment lines below to only allow delete for authorized users in Admin role
        //[Authorize(Roles = "Admin")]
        //public override HttpResponseMessage DeletePackage(string id, string version)
        //{
        //    return base.DeletePackage(id, version);
        //}
    }
}