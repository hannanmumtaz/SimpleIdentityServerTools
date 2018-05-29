namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class ProjectUnit
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string UnitId { get; set; }
        public virtual Project Project { get; set; }
        public virtual Unit Unit { get; set; }
    }
}
