using SimpleIdentityServer.Module.Feed.EF;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.Startup.Extensions
{
    internal static class ModuleFeedDbContextExtensions
    {
        public static ModuleFeedDbContext EnsureSeedData(this ModuleFeedDbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException(nameof(dbContext));
            }

            AddCategories(dbContext);
            AddProjects(dbContext);
            AddUnits(dbContext);
            AddProjectUnits(dbContext);
            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception) { }

            return dbContext;
        }

        private static void AddCategories(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Categories.Any())
            {
                dbContext.Categories.AddRange(new[]
                {
                    new PackageCategory
                    {
                        Name = "cache"
                    },
                    new PackageCategory
                    {
                        Name = "store"
                    },
                    new PackageCategory
                    {
                        Name = "ui"
                    },
                    new PackageCategory
                    {
                        Name = "handler"
                    },
                    new PackageCategory
                    {
                        Name = "bus"
                    }
                });
            }
        }

        private static void AddProjects(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Projects.Any())
            {
                dbContext.Projects.AddRange(new[]
                {
                    new Project
                    {
                        Id = "OpenIdProvider_3.0.0-rc7",
                        Version = "3.0.0-rc7",
                        ProjectName = "OpenIdProvider"
                    }
                });
            }
        }

        private static void AddUnits(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.Units.Any())
            {
                dbContext.Units.AddRange(new[]
                {
                    new Unit
                    {
                        UnitName = "openidstorage",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Library = "SimpleIdentityServer.Store.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "cache"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "openidui",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Library = "SimpleIdentityServer.Authenticate.Basic",
                                Version = "3.0.0-rc7",
                                CategoryId = "ui"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "oauthrepository",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Library = "SimpleIdentityServer.EF.SqlServer",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            }
                        }
                    },
                    new Unit
                    {
                        UnitName = "oautheventstore",
                        Packages = new []
                        {
                            new UnitPackage
                            {
                                Library = "SimpleIdentityServer.EventStore.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "store"
                            },
                            new UnitPackage
                            {
                                Library = "SimpleIdentityServer.EventStore.Handler",
                                Version = "3.0.0-rc7",
                                CategoryId = "handler"
                            },
                            new UnitPackage
                            {
                                Library = "SimpleBus.InMemory",
                                Version = "3.0.0-rc7",
                                CategoryId = "bus"
                            }
                        }
                    }
                });
            }
        }

        private static void AddProjectUnits(ModuleFeedDbContext dbContext)
        {
            if (!dbContext.ProjectUnits.Any())
            {
                dbContext.ProjectUnits.AddRange(new[]
                {
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "openidstorage"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "openidui"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "oauthrepository"
                    },
                    new ProjectUnit
                    {
                        ProjectId = "OpenIdProvider_3.0.0-rc7",
                        UnitId = "oautheventstore"
                    }
                });
            }
        }
    }
}
