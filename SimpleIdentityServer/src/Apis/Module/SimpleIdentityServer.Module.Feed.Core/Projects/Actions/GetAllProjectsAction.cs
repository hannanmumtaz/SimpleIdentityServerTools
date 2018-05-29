using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Projects.Actions
{
    public interface IGetAllProjectsAction
    {
        Task<IEnumerable<string>> Execute();
    }

    internal sealed class GetAllProjectsAction : IGetAllProjectsAction
    {
        private readonly IProjectRepository _projectRepository;

        public GetAllProjectsAction(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<string>> Execute()
        {
            var result = await _projectRepository.GetAll();
            return result.Select(p => p.ProjectName);
        }
    }
}
