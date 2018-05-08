using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.Core.Api.Claims;
using SimpleIdentityServer.ResourceManager.Core.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ClaimsController)]
    public class ClaimsController : Controller
    {
        private readonly IClaimActions _claimActions;

        public ClaimsController(IClaimActions claimActions)
        {
            _claimActions = claimActions;
        }

        [HttpGet("{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _claimActions.Get(subject, id);
                if (result == null || result.ContainsError)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(result.Content);
            }
            catch (ResourceManagerException ex)
            {
                return this.GetError(ex.Code, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpDelete("{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> Delete(string id, string url)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _claimActions.Delete(subject, id);
                if (result.ContainsError)
                {
                    var error = result.Error;
                    if (error == null)
                    {
                        error = new ErrorResponse
                        {
                            Code = Constants.ErrorCodes.InternalError,
                            Message = Constants.Errors.ErrDeleteRo
                        };
                    }

                    return new JsonResult(JsonConvert.SerializeObject(error))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                return new OkResult();
            }
            catch (ResourceManagerException ex)
            {
                return this.GetError(ex.Code, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost(".search")]
        [Authorize("connected")]
        public async Task<IActionResult> Search([FromBody] SearchClaimsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                var result = await _claimActions.Search(User.GetSubject(), request);
                if (result.ContainsError)
                {
                    var error = result.Error;
                    if (error == null)
                    {
                        error = new ErrorResponse
                        {
                            Code = Constants.ErrorCodes.InternalError,
                            Message = Constants.Errors.ErrSearchRos
                        };
                    }

                    return new JsonResult(JsonConvert.SerializeObject(error))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                return new OkObjectResult(result.Content);

            }
            catch (ResourceManagerException ex)
            {
                return this.GetError(ex.Code, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Authorize("connected")]
        public async Task<IActionResult> Add([FromBody] ClaimResponse parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            try
            {
                var result = await _claimActions.Add(User.GetSubject(), parameter);
                if (result.ContainsError)
                {
                    var error = result.Error;
                    if (error == null)
                    {
                        error = new ErrorResponse
                        {
                            Code = Constants.ErrorCodes.InternalError,
                            Message = Constants.Errors.ErrInsertRo
                        };
                    }

                    return new JsonResult(JsonConvert.SerializeObject(error))
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                }

                return new OkResult();
            }
            catch (ResourceManagerException ex)
            {
                return this.GetError(ex.Code, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
