using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Projects.Actions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Projects
{
    public interface IProjectActions
    {
        Task<IEnumerable<ProjectAggregate>> GetProject(string projectName);
        Task<ProjectAggregate> GetProject(string id, string version);
        Task<IEnumerable<string>> GetAll();
    }

    internal sealed class ProjectActions : IProjectActions
    {
        private readonly IGetProjectAction _getProjectAction;
        private readonly IGetAllProjectsAction _getAllProjectsAction;

        public ProjectActions(IGetProjectAction getProjectAction, IGetAllProjectsAction getAllProjectsAction)
        {
            _getProjectAction = getProjectAction;
            _getAllProjectsAction = getAllProjectsAction;
        }

        public Task<IEnumerable<ProjectAggregate>> GetProject(string projectName)
        {
            return _getProjectAction.Execute(projectName);
        }

        public Task<ProjectAggregate> GetProject(string projectName, string version)
        {
            return _getProjectAction.Execute(projectName, version);
        }

        public Task<IEnumerable<string>> GetAll()
        {
            return _getAllProjectsAction.Execute();
        }
    }
}
