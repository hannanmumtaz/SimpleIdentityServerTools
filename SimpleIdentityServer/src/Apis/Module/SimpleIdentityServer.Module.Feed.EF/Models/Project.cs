using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class Project
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string ProjectName { get; set; }
        public virtual ICollection<ProjectUnit> Units { get; set; }
        public virtual ICollection<Connector> Connectors { get; set; }
    }
}
