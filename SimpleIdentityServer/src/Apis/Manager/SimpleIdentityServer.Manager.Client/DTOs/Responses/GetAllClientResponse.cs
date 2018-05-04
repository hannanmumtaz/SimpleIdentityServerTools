﻿using SimpleIdentityServer.Manager.Common.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Manager.Client.DTOs.Responses
{
    public class GetAllClientResponse : BaseResponse
    {
        public GetAllClientResponse() { }

        public GetAllClientResponse(ErrorResponse error)
        {
            Error = error;
            ContainsError = true;
        }

        public IEnumerable<ClientResponse> Content { get; set; }
    }
}
