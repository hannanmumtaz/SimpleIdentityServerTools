﻿using System.Collections.Generic;

namespace SimpleIdentityServer.Profile.EF.Models
{
    public class Profile
    {
        public string Subject { get; set; }
        public string AuthUrl { get; set; }
        public string OpenIdUrl { get; set; }
        public string ScimUrl { get; set; }
    }
}
