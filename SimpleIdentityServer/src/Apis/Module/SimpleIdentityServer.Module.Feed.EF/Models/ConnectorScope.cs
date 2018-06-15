namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class ConnectorScope
    {
        public string Id { get; set; }
        public string ConnectorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Connector Connector { get; set; }
    }
}
