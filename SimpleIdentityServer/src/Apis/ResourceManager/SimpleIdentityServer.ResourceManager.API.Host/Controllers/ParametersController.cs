using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.ResourceManager.Core.Api.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ParametersController)]
    public class ParametersController : Controller
    {
        private readonly IParameterActions _parameterActions;

        public ParametersController(IParameterActions parameterActions)
        {
            _parameterActions = parameterActions;
        }

        [Authorize("connected")]
        [HttpGet("{type}")]
        public async Task<IActionResult> GetParameters(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            var result = await _parameterActions.Get(User.GetSubject(), type);
            return new OkObjectResult(result);
        }

        [Authorize("connected")]
        [HttpPut("{type}")]
        public async Task<IActionResult> UpdateParameters(string type, [FromBody] IEnumerable<UpdateParameterRequest> updateParameters)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }

            if(updateParameters == null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }
            
            await _parameterActions.Update(User.GetSubject(), updateParameters, type);
            return new OkResult();
        }
    }
}
