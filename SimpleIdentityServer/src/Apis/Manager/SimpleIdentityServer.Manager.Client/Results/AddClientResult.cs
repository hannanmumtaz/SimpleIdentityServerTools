using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Core.Common.DTOs.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class AddClientResult : BaseResponse
    {
        public ClientRegistrationResponse Content { get; set; }
    }
}
