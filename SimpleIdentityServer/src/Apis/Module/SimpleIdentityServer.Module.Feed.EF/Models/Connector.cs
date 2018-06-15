using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class Connector
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProjectId { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public string Parameters { get; set; }
        public bool IsSocial { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<ConnectorScope> Scopes { get; set; }
        public virtual ICollection<ConnectorClaimRule> ConnectorClaimRules { get; set; }
        public virtual ICollection<ConnectorClaim> ConnectorClaims { get; set; }
    }
}
