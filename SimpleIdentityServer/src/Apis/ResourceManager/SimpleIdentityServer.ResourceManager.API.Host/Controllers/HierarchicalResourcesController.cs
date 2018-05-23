﻿using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.ResourceManager.API.Host.Extensions;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using SimpleIdentityServer.ResourceManager.Core.Api.HierarchicalResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.API.Host.Controllers
{
    [Route(Constants.RouteNames.HierarchicalResourcesController)]
    public class HierarchicalResourcesController : Controller
    {
        private readonly IHierarchicalResourcesActions _hierarchicalResourcesActions;

        public HierarchicalResourcesController(IHierarchicalResourcesActions hierarchicalResourcesActions)
        {
            _hierarchicalResourcesActions = hierarchicalResourcesActions;
        }

        [HttpGet("{id}/{includeChildren?}")]
        public async Task<IActionResult> Get(string id, bool? includeChildren)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var assets = await _hierarchicalResourcesActions.Get(id, includeChildren == null ? false : includeChildren.Value);
            var result = new List<AssetResponse>();
            if (assets != null && assets.Any())
            {
                foreach(var asset in assets)
                {
                    result.Add(asset.ToDto());
                }
            }

            return new OkObjectResult(result);
        }
    }
}
