using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim;
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
    }
}
