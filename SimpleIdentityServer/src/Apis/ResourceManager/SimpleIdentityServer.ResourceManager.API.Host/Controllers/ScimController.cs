using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim;
using SimpleIdentityServer.Scim.Client;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ScimController)]
    public class ScimController : Controller
    {
        private readonly IScimActions _scimActions;

        public ScimController(IScimActions scimActions)
        {
            _scimActions = scimActions;
        }

        [HttpGet("schemas")]
        [Authorize("connected")]
        public async Task<IActionResult> GetSchemas()
        {
            var subject = User.GetSubject();
            var result = await _scimActions.GetSchemas(subject);
            return new OkObjectResult(result);
        }

        [HttpPost("users/.search")]
        [Authorize("connected")]
        public async Task<IActionResult> SearchUsers([FromBody] SearchParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var subject = User.GetSubject();
            var result = await _scimActions.SearchUsers(subject, parameter);
            return new OkObjectResult(result);
        }

        [HttpPost("groups/.search")]
        [Authorize("connected")]
        public async Task<IActionResult> SearchGroups([FromBody] SearchParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            var subject = User.GetSubject();
            var result = await _scimActions.SearchGroups(subject, parameter);
            return new OkObjectResult(result);
        }

        [HttpGet("users/{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> GetUser(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var subject = User.GetSubject();
            var result = await _scimActions.GetUser(subject, id);
            return new OkObjectResult(result);
        }

        [HttpGet("groups/{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> GetGroup(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var subject = User.GetSubject();
            var result = await _scimActions.GetGroup(subject, id);
            return new OkObjectResult(result);
        }
    }
}
