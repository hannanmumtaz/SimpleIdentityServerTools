﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Manager.Common.Requests;
using SimpleIdentityServer.Manager.Common.Responses;
using SimpleIdentityServer.Manager.Core.Api.Claims;
using SimpleIdentityServer.Manager.Host.Extensions;
using System;
using System.Threading.Tasks;
using WebApiContrib.Core.Concurrency;

namespace SimpleIdentityServer.Manager.Host.Controllers
{
    [Route(Constants.EndPoints.Claims)]
    public class ClaimsController : Controller
    {
        public const string GetClaimsStoreName = "GetClaims";
        public const string GetClaimStoreName = "GetClaim_";
        private readonly IClaimActions _claimActions;
        private readonly IRepresentationManager _representationManager;

        public ClaimsController(IClaimActions claimActions, IRepresentationManager representationManager)
        {
            _claimActions = claimActions;
            _representationManager = representationManager;
        }

        [HttpGet("{id}")]
        [Authorize("manager")]
        public async Task<ActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!await _representationManager.CheckRepresentationExistsAsync(this, GetClaimStoreName + id))
            {
                return new ContentResult
                {
                    StatusCode = 412
                };
            }

            var result = await _claimActions.Get(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            var response = result.ToDto();
            await _representationManager.AddOrUpdateRepresentationAsync(this, GetClaimStoreName + id);
            return new OkObjectResult(response);
        }

        [HttpGet]
        [Authorize("manager")]
        public async Task<ActionResult> GetAll()
        {
            if (!await _representationManager.CheckRepresentationExistsAsync(this, GetClaimsStoreName))
            {
                return new ContentResult
                {
                    StatusCode = 412
                };
            }

            var result = (await _claimActions.GetAll()).ToDtos();
            await _representationManager.AddOrUpdateRepresentationAsync(this, GetClaimsStoreName);
            return new OkObjectResult(result);
        }

        [HttpPost(".search")]
        [Authorize("manager")]
        public async Task<ActionResult> Search([FromBody] SearchClaimsRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var parameter = request.ToParameter();
            var result = await _claimActions.Search(parameter);
            return new OkObjectResult(result.ToDto());
        }
        
        [HttpDelete("{id}")]
        [Authorize("manager")]
        public async Task<ActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!await _claimActions.Delete(id))
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            await _representationManager.AddOrUpdateRepresentationAsync(this, GetClaimStoreName + id, false);
            await _representationManager.AddOrUpdateRepresentationAsync(this, GetClaimsStoreName, false);
            return new NoContentResult();
        }

        [HttpPost]
        [Authorize("manager")]
        public async Task<ActionResult> Add([FromBody] ClaimResponse claim)
        {
            if (claim == null)
            {
                throw new ArgumentNullException(nameof(claim));
            }

            var result = await _claimActions.Add(claim.ToParameter());
            await _representationManager.AddOrUpdateRepresentationAsync(this, GetClaimStoreName, false);
            return new OkObjectResult(result);
        }
    }
}
