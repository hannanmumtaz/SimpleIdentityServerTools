using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Core.Jwks;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Api.Controllers
{
    [Route(Constants.RouteNames.Jwks)]
    public class JwksController : Controller
    {
        private readonly IJwksActions _jwksActions;

        public JwksController(IJwksActions jwksActions)
        {
            _jwksActions = jwksActions;
        }
        
        [HttpGet]
        public Task<JsonWebKeySet> Get()
        {
            return _jwksActions.GetJwks();
        }
    }
}
