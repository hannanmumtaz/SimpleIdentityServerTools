using SimpleIdentityServer.Common.Dtos.Responses;
using System.Net;

namespace SimpleIdentityServer.Common.Client
{
    public class BaseResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaWellKnownConfiguration { get; set; }
        public bool ContainsError { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
