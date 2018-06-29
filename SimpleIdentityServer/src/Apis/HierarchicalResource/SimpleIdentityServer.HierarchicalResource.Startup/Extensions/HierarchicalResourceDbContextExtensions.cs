using SimpleIdentityServer.HierarchicalResource.EF;
using SimpleIdentityServer.HierarchicalResource.EF.Models;
using SimpleIdentityServer.HierarchicalResource.Host.Helpers;
using System;
using System.Linq;

namespace SimpleIdentityServer.HierarchicalResource.Startup.Extensions
{
    internal static class HierarchicalResourceDbContextExtensions
    {
        public static void EnsureSeedData(this HierarchicalResourceDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            AddAssets(dbContext);
            dbContext.SaveChanges();
        }

        private static void AddAssets(HierarchicalResourceDbContext context)
        {
            if (!context.Assets.Any())
            {
                context.Assets.AddRange(new[]
                {
                   new Asset
                   {
                       Hash = HashHelper.GetHash("ProtectedWebsite"),
                       Name = "ProtectedWebsite",
                       Path = "ProtectedWebsite",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       IsDefaultWorkingDirectory = true,
                       MimeType = "directory"
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("ProtectedWebsite/Administrator"),
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite"),
                       Name = "Administrator",
                       Path = "ProtectedWebsite/Administrator",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("ProtectedWebsite/Home"),
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite"),
                       Name = "Home",
                       Path = "ProtectedWebsite/Home",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("ProtectedWebsite/About"),
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite"),
                       Name = "About",
                       Path = "ProtectedWebsite/About",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("ProtectedWebsite/Administrator/Index"),
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite/Administrator"),
                       Name = "Index",
                       Path = "ProtectedWebsite/Administrator/Index",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "text/plain",
                       ResourceId = "1"
                   }
                });
            }
        }
    }
}
