﻿using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class PagedResult<T> : BaseResponse
    {
        public PagedResponse<T> Content { get; set; }
    }
}
