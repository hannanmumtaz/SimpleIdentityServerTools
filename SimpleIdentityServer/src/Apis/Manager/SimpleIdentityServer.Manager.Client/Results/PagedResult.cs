using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class PagedResult<T> : BaseManagerResult
    {
        public PagedResponse<T> Content { get; set; }
    }
}
