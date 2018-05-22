﻿using SimpleIdentityServer.ResourceManager.API.Host.Helpers;
using SimpleIdentityServer.ResourceManager.Core.Models;
using SimpleIdentityServer.ResourceManager.EF;
using SimpleIdentityServer.ResourceManager.EF.Models;
using System;
using System.Linq;

namespace SimpleIdentityServer.ResourceManager.API.Host.Extensions
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
                       Hash = HashHelper.GetHash("Root"),
                       Name = "Root",
                       Path = "Root",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       IsDefaultWorkingDirectory = true,
                       MimeType = Constants.MimeNames.Directory
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("Root/Sub"),
                       ResourceParentHash = HashHelper.GetHash("Root"),
                       Name = "Sub",
                       Path = "Root/Sub",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = Constants.MimeNames.Directory
                   },
                   new Asset
                   {
                       Hash = HashHelper.GetHash("Root/another directory"),
                       ResourceParentHash = HashHelper.GetHash("Root"),
                       Name = "another directory",
                       Path = "Root/another directory",
                       IsLocked = false,
                       CanRead = true,
                       CanWrite = true,
                       CreateDateTime = DateTime.UtcNow,
                       MimeType = Constants.MimeNames.Directory
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
