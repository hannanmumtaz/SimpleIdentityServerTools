using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Feed.Core.Projects;
using SimpleIdentityServer.Module.Feed.Host.Extensions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Host.Controllers
{
    [Route(Constants.Routes.ProjectsController)]
    public class ProjectsController : Controller
    {
        private readonly IProjectActions _projectActions;

        public ProjectsController(IProjectActions projectActions)
        {
            _projectActions = projectActions;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _projectActions.GetAll();
            return new OkObjectResult(result);
        }

        [HttpGet("{projectName}")]
        public async Task<IActionResult> Get(string projectName)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            var result = await _projectActions.GetProject(projectName);
            var projectReponses = new List<ProjectResponse>();
            foreach(var record in result)
            {
                projectReponses.Add(record.ToDto());
            }

            return new OkObjectResult(projectReponses);
        }

        [HttpGet("{projectName}/{version}")]
        public async Task<IActionResult> Get(string projectName, string version)
        {
            if (string.IsNullOrWhiteSpace(projectName))
            {
                throw new ArgumentNullException(nameof(projectName));
            }

            if (string.IsNullOrWhiteSpace(version))
            {
                throw new ArgumentNullException(nameof(version));
            }

            var result = await _projectActions.GetProject(projectName, version);
            if (result == null)
            {
                return NotFound();
            }

            return new OkObjectResult(result.ToDto());
        }
    }
}