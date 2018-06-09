using SimpleIdentityServer.Common.Dtos.Responses;

namespace SimpleIdentityServer.Common.Client
{
    public class BaseResponse
    {
        public bool ContainsError { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaWellKnownConfiguration { get; set; }
        public ErrorResponse Error { get; set; }
    }
}
