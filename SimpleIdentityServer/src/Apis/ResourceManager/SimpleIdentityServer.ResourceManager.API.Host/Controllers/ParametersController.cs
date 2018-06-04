using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ParametersController)]
    public class ParametersController : Controller
    {
        public async Task<IActionResult> GetParameter(string type)
        {
            return null;
        }
    }
}
