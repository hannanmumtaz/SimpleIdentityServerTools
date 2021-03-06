﻿using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Manager.Common.Responses;

namespace SimpleIdentityServer.Manager.Client.Results
{
    public class GetResourceOwnerResult : BaseResponse
    {
        public ResourceOwnerResponse Content { get; set; }
    }
}
