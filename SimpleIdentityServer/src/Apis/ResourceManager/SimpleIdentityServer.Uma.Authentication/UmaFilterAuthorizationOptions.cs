﻿namespace SimpleIdentityServer.Uma.Authentication
{
    public class UmaFilterAuthorizationOptions
    {
        public string AuthorizationWellKnownConfiguration { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}