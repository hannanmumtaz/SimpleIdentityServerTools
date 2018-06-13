using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.Module.Feed.Common.Requests;
using SimpleIdentityServer.Module.Feed.Core.Connectors;
using SimpleIdentityServer.Module.Feed.Core.Exceptions;
using SimpleIdentityServer.Module.Feed.Host.Extensions;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Module.Feed.Host.Controllers
{
    [Route(Constants.Routes.ConnectorsController)]
    public class ConnectorsController : Controller
    {
        private readonly IConnectorActions _connectorActions;

        public ConnectorsController(IConnectorActions connectorActions)
        {
            _connectorActions = connectorActions;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _connectorActions.GetAll();
            return new OkObjectResult(result.Select(r => r.ToDto()));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var connector = await _connectorActions.Get(id);
            if (connector == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(connector.ToDto());
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddConnectorRequest addConnectorRequest)
        {
            if (addConnectorRequest == null)
            {
                throw new ArgumentNullException(nameof(addConnectorRequest));
            }

            ErrorResponse error = null;
            try
            {
                if (await _connectorActions.Add(addConnectorRequest.ToParameter()))
                {
                    return new OkResult();
                }

                error = new ErrorResponse
                {
                    Code = "internal",
                    Message = "cannot_add"
                };
            }
            catch(BaseModuleFeedException ex)
            {
                error = new ErrorResponse
                {
                    Code = ex.Code,
                    Message = ex.Message
                };
            }
            catch(Exception ex)
            {
                error = new ErrorResponse
                {
                    Code = "internal",
                    Message = ex.Message
                };
            }

            return new JsonResult(JsonConvert.SerializeObject(error))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            ErrorResponse error = null;
            try
            {
                if (await _connectorActions.Delete(id))
                {
                    return new OkResult();
                }

                error = new ErrorResponse
                {
                    Code = "internal",
                    Message = "cannot_update"
                };
            }
            catch (NoConnectorException)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                error = new ErrorResponse
                {
                    Code = "internal",
                    Message = ex.Message
                };
            }

            return new JsonResult(JsonConvert.SerializeObject(error))
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }
    }
}
