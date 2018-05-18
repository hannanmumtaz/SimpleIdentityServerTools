using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.Extensions;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.Core.Api.AuthPolicies;
using SimpleIdentityServer.ResourceManager.Core.Exceptions;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.AuthPoliciesController)]
    public class AuthPoliciesController : Controller
    {
        private readonly IAuthorizationPolicyActions _authorizationPolicyActions;

        public AuthPoliciesController(IAuthorizationPolicyActions authorizationPolicyActions)
        {
            _authorizationPolicyActions = authorizationPolicyActions;
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
                await _authorizationPolicyActions.Delete(subject, id);
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
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] PutPolicy putPolicy)
        {
            if (putPolicy == null)
            {
                throw new ArgumentNullException(nameof(putPolicy));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _authorizationPolicyActions.Update(subject, putPolicy);
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
        public async Task<IActionResult> Add([FromBody] PostPolicy postPolicy)
        {
            if (postPolicy == null)
            {
                throw new ArgumentNullException(nameof(postPolicy));
            }

            try
            {
                var subject = User.GetSubject();
                var result = await _authorizationPolicyActions.Add(subject, postPolicy);
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
    }
}
