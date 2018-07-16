using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.EF;
using SimpleIdentityServer.Profile.EF.Models;
using System;
using System.Linq;

namespace SimpleIdentityServer.Merged.API.Startup.Extensions
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
                        ManagerUrl = "http://localhost:60006/openidmanager/.well-known/openidmanager-configuration",
                        Order = 1
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "authorization server",
                        Name = "authserver",
                        Url = "http://localhost:60004/.well-known/uma2-configuration",
                        Type = (int)EndpointTypes.AUTH,
                        ManagerUrl = "http://localhost:60006/oauthmanager/.well-known/openidmanager-configuration",
                        Order = 1
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "scim server",
                        Name = "scimserver",
                        Url = "http://localhost:60001",
                        Type = (int)EndpointTypes.SCIM,
                        ManagerUrl = "http://localhost:60004/.well-known/uma2-configuration",
                        Order = 1
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "fake simple identity server",
                        Name = "fakesimpleidserver",
                        Url = "http://fake:60000/.well-known/openid-configuration",
                        Type = (int)EndpointTypes.OPENID,
                        ManagerUrl = "http://fake:60003/.well-known/openidmanager-configuration",
                        Order = 2
                    },
                    new Endpoint
                    {
                        CreateDateTime = DateTime.UtcNow,
                        Description = "fake authorization server",
                        Name = "fakeauthserver",
                        Url = "http://fake:60004/.well-known/uma2-configuration",
                        ManagerUrl = "http://fake:60004/.well-known/uma2-configuration",
                        Type = (int)EndpointTypes.AUTH,
                        Order = 1
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
