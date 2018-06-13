using System;

namespace SimpleIdentityServer.Module.Feed.Core.Models
{
    public class ConnectorAggregate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public string Parameters { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
