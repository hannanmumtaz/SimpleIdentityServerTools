using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Parameters;
using SimpleIdentityServer.Parameter.Host.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace SimpleIdentityServer.Parameter.Host.Controllers
{
    [Route("parameters")]
    public class ParametersController : Controller
    {
        private readonly IParameterActions _parameterActions;

        public ParametersController(IParameterActions parameterActions)
        {
            _parameterActions = parameterActions;
        }

        [HttpGet("modules")]
        [Authorize("get")]
        public IActionResult GetModules()
        {
            var modules = _parameterActions.GetModules();
            var result = modules.ToDto();
            return new OkObjectResult(result);
        }

        [HttpGet("connectors")]
        [Authorize("get")]
        public IActionResult GetConnectors()
        {
            var connectors = _parameterActions.GetConnectors();
            var result = connectors.ToDto();
            return new OkObjectResult(connectors);
        }

        [HttpPut("modules")]
        [Authorize("add")]
        public IActionResult UpdateModules([FromBody] IEnumerable<UpdateParameterRequest> updateParametersRequest)
        {
            if (updateParametersRequest == null)
            {
                throw new ArgumentNullException(nameof(updateParametersRequest));
            }

            try
            {
                var parameters = updateParametersRequest.Select(u => u.ToParameter());
                _parameterActions.Update(parameters);
                return new OkResult();

            }
            catch(BadConfigurationException ex)
            {
                return BuildError(new ErrorResponse
                {
                    Code = "configuration",
                    Message = ex.Message
                }, HttpStatusCode.InternalServerError);
            }
            catch(ParameterAggregateException ex)
            {
                return BuildError(new ErrorResponse
                {
                    Code = "validation",
                    Message = string.Join(",", ex.Messages)
                }, HttpStatusCode.InternalServerError);
            }
            catch(Exception ex)
            {
                return BuildError(new ErrorResponse
                {
                    Code = "internal",
                    Message = ex.Message
                }, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost("connectors")]
        [Authorize("add")]
        public IActionResult UpdateConnectors([FromBody] IEnumerable<UpdateConnectorRequest> updateConnectorsRequest)
        {
            if (updateConnectorsRequest == null)
            {
                throw new ArgumentNullException(nameof(updateConnectorsRequest));
            }

            _parameterActions.Update(updateConnectorsRequest.Select(u => u.ToParameter()));
            return new OkResult();
        }

        private IActionResult BuildError(ErrorResponse error, HttpStatusCode httpStatusCode)
        {
            return new ObjectResult(error)
            {
                StatusCode = (int)httpStatusCode
            };
        }
    }
}
