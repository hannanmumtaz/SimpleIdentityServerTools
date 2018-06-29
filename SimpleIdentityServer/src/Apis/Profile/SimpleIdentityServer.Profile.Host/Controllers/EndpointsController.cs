using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SimpleIdentityServer.Profile.Host.Extensions;
using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.Core.Parameters;
using SimpleIdentityServer.Profile.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Profile.Host.Controllers
{
    [Route(Constants.RouteNames.EndpointsController)]
    public class EndpointsController : Controller
    {
        private readonly IEndpointRepository _endpointRepository;

        public EndpointsController(IEndpointRepository endpointRepository)
        {
            _endpointRepository = endpointRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _endpointRepository.GetAll();
            return new OkObjectResult(ToJson(result));
        }

        [HttpPost(".search")]
        public async Task<IActionResult> Search([FromBody] JObject jObj)
        {
            if (jObj == null)
            {
                throw new ArgumentNullException(nameof(jObj));
            }

            EndpointTypes type;
            JToken jType;
            if (!jObj.TryGetValue(Constants.EndpointNames.Type, out jType))
            {
                return this.GetError(string.Format(Constants.Errors.ErrParamNotSpecified, Constants.EndpointNames.Type), HttpStatusCode.InternalServerError);
            }

            if (!Enum.TryParse(jType.ToString(), out type))
            {
                return this.GetError(string.Format(Constants.Errors.ErrParamNotSpecified, Constants.EndpointNames.Type), HttpStatusCode.InternalServerError);
            }

            var parameter = new SearchEndpointsParameter
            {
                Type = type
            };

            var result = await _endpointRepository.Search(parameter);
            return new OkObjectResult(ToJson(result));
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var result = await _endpointRepository.Get(id);
            if (result == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(ToJson(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (!await _endpointRepository.Remove(id))
            {
                return this.GetError(Constants.Errors.ErrRemoveEndpoint, HttpStatusCode.InternalServerError);
            }

            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] JObject jObj)
        {
            if (jObj == null)
            {
                throw new ArgumentNullException(nameof(jObj));
            }

            // Check the url is a correct well-known configuration.
            // Check the manager url is correct.
            // Check the client id + secret are correct.
            return null;
        }

        private static JArray ToJson(IEnumerable<EndpointAggregate> endpoints)
        {
            if (endpoints == null)
            {
                throw new ArgumentNullException(nameof(endpoints));
            }

            var jArr = new JArray();
            foreach(var endpoint in endpoints)
            {
                jArr.Add(ToJson(endpoint));
            }

            return jArr;
        }

        private static JObject ToJson(EndpointAggregate endpoint)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }

            var jObj = new JObject();
            jObj.Add(Constants.EndpointNames.CreateDateTime, endpoint.CreateDateTime);
            jObj.Add(Constants.EndpointNames.Description, endpoint.Description);
            jObj.Add(Constants.EndpointNames.Name, endpoint.Name);
            jObj.Add(Constants.EndpointNames.Type, (int)endpoint.Type);
            jObj.Add(Constants.EndpointNames.Url, endpoint.Url);
            jObj.Add(Constants.EndpointNames.ManagerUrl, endpoint.ManagerUrl);
            return jObj;
        }
    }
}