using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.EF;
using SimpleIdentityServer.Profile.EF.Models;
using System;
using System.Linq;

namespace SimpleIdentityServer.Profile.Startup.Extensions
{
    internal static class ProfileDbContextExtensions
    {
        public static void EnsureSeedData(this ProfileDbContext profileDbContext)
        {
            if (profileDbContext == null)
            {
                throw new ArgumentNullException(nameof(profileDbContext));
            }

            AddIdProviders(profileDbContext);
            profileDbContext.SaveChanges();
        }

        private static void AddIdProviders(ProfileDbContext context)
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
                            ManagerUrl = "http://localhost:60007/.well-known/openidmanager-configuration"
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
