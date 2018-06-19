using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Parameters;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using SimpleIdentityServer.Module.Feed.EF.Extensions;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.EF.Repositories
{
    internal sealed class ProjectRepository : IProjectRepository
    {
        private readonly IServiceProvider _serviceProvider;

        public ProjectRepository(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ProjectAggregate> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    var project = await context.Projects
                        .Include(a => a.Connectors)
                        .Include(a => a.TwoFactorAuthenticators)
                        .Include(a => a.Units).ThenInclude(a => a.Unit).ThenInclude(a => a.Packages).ThenInclude(a => a.Category)
                        .FirstOrDefaultAsync(p => p.Id == id).ConfigureAwait(false);
                    if (project == null)
                    {
                        return null;
                    }

                    return project.ToDomain();
                }
            }
        }

        public async Task<IEnumerable<ProjectAggregate>> GetAll()
        {
            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    var projects = await context.Projects
                        .Include(a => a.Connectors)
                        .Include(a => a.TwoFactorAuthenticators)
                        .Include(a => a.Units).ThenInclude(a => a.Unit).ThenInclude(a => a.Packages).ThenInclude(a => a.Category)
                        .ToListAsync().ConfigureAwait(false);
                    return projects == null ? new List<ProjectAggregate>() : projects.Select(p => p.ToDomain());
                }
            }
        }

        public async Task<IEnumerable<ProjectAggregate>> Search(SearchProjectsParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            using (var serviceScope = _serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ModuleFeedDbContext>())
                {
                    IQueryable<Project> projects = context.Projects
                        .Include(a => a.Connectors)
                        .Include(a => a.TwoFactorAuthenticators)
                        .Include(a => a.Units).ThenInclude(a => a.Unit).ThenInclude(a => a.Packages).ThenInclude(a => a.Category);
                    if (parameter.ProjectNames != null)
                    {
                        projects = projects.Where(p => parameter.ProjectNames.Contains(p.ProjectName));
                    }

                    var result = await projects.ToListAsync().ConfigureAwait(false);
                    return result == null ? new List<ProjectAggregate>() : result.Select(p => p.ToDomain());
                }
            }
        }
    }
}
