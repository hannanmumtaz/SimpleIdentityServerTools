using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.Core.Api.Resources;
using SimpleIdentityServer.ResourceManager.Core.Exceptions;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.ResourcesController)]
    public class ResourcesController : Controller
    {
        private readonly IResourcesetActions _resourcesetActions;

        public ResourcesController(IResourcesetActions resourcesetActions)
        {
            _resourcesetActions = resourcesetActions;
        }

        [Authorize("connected")]
        [HttpPost(".search")]
        public async Task<IActionResult> Search([FromBody] SearchResourceSet parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _resourcesetActions.Search(subject, parameter);
                return new OkObjectResult(result);
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

        [Authorize("connected")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _resourcesetActions.Get(subject, id);
                return new OkObjectResult(result);
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

        [Authorize("connected")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var subject = User.GetSubject();
                await _resourcesetActions.Delete(subject, id);
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

        [Authorize("connected")]
        [HttpGet("{id}/policies")]
        public async Task<IActionResult> GetAuthPolicies(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _resourcesetActions.GetAuthPolicies(subject, id);
                return new OkObjectResult(result);
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

        [Authorize("connected")]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] PostResourceSet postResourceSet)
        {
            if (postResourceSet == null)
            {
                throw new ArgumentNullException(nameof(postResourceSet));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _resourcesetActions.Add(subject, postResourceSet);
                return new OkObjectResult(result);
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

        [Authorize("connected")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PutResourceSet putResourceSet)
        {
            if (putResourceSet == null)
            {
                throw new ArgumentNullException(nameof(putResourceSet));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _resourcesetActions.Update(subject, putResourceSet);
                return new OkObjectResult(result);
            }
            catch(ResourceManagerException ex)
            {
                return this.GetError(ex.Code, ex.Message, HttpStatusCode.InternalServerError);
            }
            catch(Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
