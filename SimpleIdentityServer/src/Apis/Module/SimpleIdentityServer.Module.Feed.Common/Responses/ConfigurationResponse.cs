﻿using System.Runtime.Serialization;

namespace SimpleIdentityServer.Module.Feed.Common.Responses
{
    [DataContract]
    public class ConfigurationResponse
    {
        [DataMember(Name = "projects_edp")]
        public string ProjectsEndpoint { get; set; }
        [DataMember(Name = "connectors_edp")]
        public string ConnectorsEndpoint { get; set; }
    }
}
