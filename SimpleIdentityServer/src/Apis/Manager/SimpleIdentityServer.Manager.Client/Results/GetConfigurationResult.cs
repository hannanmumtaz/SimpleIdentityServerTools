using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetConfigurationResult : BaseResponse
    {
	    public ConfigurationResponse Content { get; set; }
    }
}
