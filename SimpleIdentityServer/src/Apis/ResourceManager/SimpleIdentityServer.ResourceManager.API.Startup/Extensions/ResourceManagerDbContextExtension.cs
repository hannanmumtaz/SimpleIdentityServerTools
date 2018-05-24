using SimpleIdentityServer.ResourceManager.API.Host.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.EF;
using SimpleIdentityServer.ResourceManager.EF.Models;
using System;
using System.Linq;

namespace SimpleIdentityServer.ResourceManager.API.Startup.Extensions
{
    internal static class ResourceManagerDbContextExtension
    {
        public static void EnsureSeedData(this ResourceManagerDbContext resourceManagerContext)
        {
            if (resourceManagerContext == null)
            {
                throw new ArgumentNullException(nameof(resourceManagerContext));
            }

            AddAssets(resourceManagerContext);
            AddAssetAuthPolicies(resourceManagerContext);
            AddIdProviders(resourceManagerContext);
            resourceManagerContext.SaveChanges();
        }

        private static void AddAssets(ResourceManagerDbContext context)
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
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite/Home"),
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
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite/About"),
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
                       ResourceParentHash = HashHelper.GetHash("ProtectedWebsite/Administrator/Index"),
                       Name = "aIndex",
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

        private static void AddAssetAuthPolicies(ResourceManagerDbContext context)
        {
            if (!context.Assets.Any())
            {
            }
        }

        private static void AddIdProviders(ResourceManagerDbContext context)
        {
            if (!context.Endpoints.Any())
            {
                context.Endpoints.AddRange(new[]
                {
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "simple identity server",
                        Name = "simpleidserver",
                        Url = "http://localhost:60000/.well-known/openid-configuration",
                        Type = (int)EndpointTypes.OPENID,
                        Manager = new EndpointManager
                        {
                            AuthUrl = "http://localhost:60004/.well-known/uma2-configuration",
                            ClientId = "ResourceServer",
                            ClientSecret = "LW46am54neU/[=Su",
                            ManagerUrl = "http://localhost:60003/.well-known/openidmanager-configuration"
                        },
                        Order = 1
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "authorization server",
                        Name = "authserver",
                        Url = "http://localhost:60004/.well-known/uma2-configuration",
                        Type = (int)EndpointTypes.AUTH,
                        Order = 1,
                        Manager = new EndpointManager
                        {
                            AuthUrl = "http://localhost:60004/.well-known/uma2-configuration",
                            ClientId = "ResourceServer",
                            ClientSecret = "LW46am54neU/[=Su",
                            ManagerUrl = "http://localhost:60004/.well-known/uma2-configuration"
                        }
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "scim server",
                        Name = "scimserver",
                        Url = "http://localhost:60001",
                        Type = (int)EndpointTypes.SCIM,
                        Order = 1,
                        Manager = new EndpointManager
                        {
                            AuthUrl = "http://localhost:60004/.well-known/uma2-configuration",
                            ClientId = "ResourceServer",
                            ClientSecret = "LW46am54neU/[=Su",
                            ManagerUrl = "http://localhost:60004/.well-known/uma2-configuration"
                        }
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "fake simple identity server",
                        Name = "fakesimpleidserver",
                        Url = "http://fake:60000/.well-known/openid-configuration",
                        Type = (int)EndpointTypes.OPENID,
                        Manager = new EndpointManager
                        {
                            AuthUrl = "http://fake:60004/.well-known/uma2-configuration",
                            ClientId = "ResourceServer",
                            ClientSecret = "LW46am54neU/[=Su",
                            ManagerUrl = "http://fake:60003/.well-known/openidmanager-configuration"
                        },
                        Order = 2
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "fake authorization server",
                        Name = "fakeauthserver",
                        Url = "http://fake:60004/.well-known/uma2-configuration",
                        Type = (int)EndpointTypes.AUTH,
                        Order = 1,
                        Manager = new EndpointManager
                        {
                            AuthUrl = "http://fake:60004/.well-known/uma2-configuration",
                            ClientId = "ResourceServer",
                            ClientSecret = "LW46am54neU/[=Su",
                            ManagerUrl = "http://fake:60004/.well-known/uma2-configuration"
                        }
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "fakescim server",
                        Name = "fakescimserver",
                        Url = "http://fake:60001/ServiceProviderConfig",
                        Type = (int)EndpointTypes.SCIM,
                        Order = 1
                    }
                });
            }
        }
    }
}
