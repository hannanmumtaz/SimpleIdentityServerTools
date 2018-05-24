﻿using SimpleIdentityServer.Common.Dtos.Responses;

namespace SimpleIdentityServer.Common.Client
{
    public class BaseResponse
    {
        public bool ContainsError { get; set; }
        public ErrorResponse Error { get; set; }
    }
}