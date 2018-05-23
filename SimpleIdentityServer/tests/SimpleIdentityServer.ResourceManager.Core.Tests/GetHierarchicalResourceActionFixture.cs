using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources.Actions;
using SimpleIdentityServer.ResourceManager.Core.Repositories;
using SimpleIdentityServer.ResourceManager.EF;
using SimpleIdentityServer.ResourceManager.EF.InMemory;
using SimpleIdentityServer.ResourceManager.EF.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.ResourceManager.Core.Tests
{
    public class GetHierarchicalResourceActionFixture
    {
        [Fact]
        public async Task WhenPassNullParameterThenExceptionIsThrown()
        {
            // ARRANGE
            var getHierarchicalResourceAction = new GetHierarchicalResourceAction(null);

            // ACT & ASSERT
            await Assert.ThrowsAsync<ArgumentNullException>(() => getHierarchicalResourceAction.Execute(null));
        }
            
        [Fact]
        public async Task WhenSearchResourceThenFiveRecordsAreReturned()
        {
            // ARRANGE
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddResourceManagerInMemoryEF();
            var builder = serviceCollection.BuildServiceProvider();
            var context = builder.GetService<ResourceManagerDbContext>();
            var assetRepository = builder.GetService<IAssetRepository>();
            context.Assets.AddRange(new[]
            {
                new Asset
                {
                       Hash = "Root",
                       Name = "Root",
                       Path = "Root",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       IsDefaultWorkingDirectory = true,
                       MimeType = "directory"
                },
                new Asset
                {
                       Hash = "Root/Sub",
                       ResourceParentHash = "Root",
                       Name = "Sub",
                       Path = "Root/Sub",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                },
                new Asset
                {
                       Hash = "Root/Sub/Sub1",
                       ResourceParentHash = "Root/Sub",
                       Name = "Sub1",
                       Path = "Root/Sub/Sub1",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                },
                new Asset
                {
                       Hash = "Root/Sub2",
                       ResourceParentHash = "Root",
                       Name = "Sub2",
                       Path = "Root/Sub2",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = "directory"
                },
            });
            await context.SaveChangesAsync();
            var getHierarchicalResourceAction = new GetHierarchicalResourceAction(assetRepository);

            // ACT
            var result = await getHierarchicalResourceAction.Execute("Root", true);

            string s = "";
        }
    }
}