using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;

namespace SimpleIdentityServer.DocumentManagement.Api.Controllers
{
    [Route(Constants.RouteNames.Configuration)]
    public class ConfigurationController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var issuer = Request.GetAbsoluteUriWithVirtualPath();
            var result = new OfficeDocumentConfigurationResponse
            {
                JwksEndpoint = issuer + "/" + Constants.RouteNames.Jwks,
                OfficeDocumentsEndpoint = issuer + "/" + Constants.RouteNames.OfficeDocuments
            };
            return new OkObjectResult(result);
        }
    }
}
