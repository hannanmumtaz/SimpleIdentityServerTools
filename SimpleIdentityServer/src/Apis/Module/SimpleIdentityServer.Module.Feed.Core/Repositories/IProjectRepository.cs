using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Repositories
{
    public interface IProjectRepository
    {
        Task<ProjectAggregate> Get(string id);
        Task<IEnumerable<ProjectAggregate>> GetAll();
        Task<IEnumerable<ProjectAggregate>> Search(SearchProjectsParameter parameter);
    }
}