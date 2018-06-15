namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class ConnectorClaimRule
    {
        public string Id { get; set; }
        public string ConnectorId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public virtual Connector Connector { get; set; }
    }
}
