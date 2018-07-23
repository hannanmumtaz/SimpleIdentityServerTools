using SimpleIdentityServer.Core.Common.DTOs.Responses;
using System.Net;

namespace SimpleIdentityServer.Manager.Client
{
    public class BaseManagerResult
    {
        public bool ContainsError { get; set; }
        public ErrorResponseWithState Error { get; set; }
        public HttpStatusCode Status { get; set; }
    }
}
