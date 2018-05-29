using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.Core.Parameters;
using SimpleIdentityServer.Module.Feed.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Core.Projects.Actions
{
    public interface IGetProjectAction
    {
        Task<IEnumerable<ProjectAggregate>> Execute(string projectName);
        Task<ProjectAggregate> Execute(string projectName, string version);
    }

    internal class GetProjectAction : IGetProjectAction
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectAction(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public Task<IEnumerable<ProjectAggregate>> Execute(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }
            
            return _projectRepository.Search(new SearchProjectsParameter
            {
                ProjectNames = new []
                {
                    projectName
                }
            });
        }

        public Task<ProjectAggregate> Execute(string projectName, string version)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            return _projectRepository.Get($"{projectName}_{version}");
        }
    }
}
