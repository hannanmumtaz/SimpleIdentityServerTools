using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class ConnectorClaim
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string ConnectorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Connector Connector { get; set; }
        public virtual ConnectorClaim ParentConnector { get; set; }
        public virtual ICollection<ConnectorClaim> Children { get; set; }
    }
}
