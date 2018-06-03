using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Configuration.Host.Controllers
{
    public class ParametersController : Controller
    {
        public ParametersController()
        {

        }

        [HttpGet("modules")]
        public async Task<IActionResult> GetModules()
        {
            return null;
        }

        [HttpGet("connectors")]
        public async Task<IActionResult> GetConnectors()
        {
            return null;
        }
    }
}
