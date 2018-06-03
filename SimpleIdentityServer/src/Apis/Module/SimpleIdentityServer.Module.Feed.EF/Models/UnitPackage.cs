namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class UnitPackage
    {
        public string Id { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public string CategoryId { get; set; }
        public string UnitName { get; set; }
        public string Parameters { get; set; }
        public virtual PackageCategory Category { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
