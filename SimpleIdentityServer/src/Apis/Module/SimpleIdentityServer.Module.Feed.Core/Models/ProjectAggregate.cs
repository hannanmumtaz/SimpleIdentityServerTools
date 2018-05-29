using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.Core.Models
{
    public class UnitPackageAggregate
    {
        public string Library { get; set; }
        public string Version { get; set; }
        public string CategoryName { get; set; }
    }
        
    public class ProjectUnitAggregate
    {
        public string UnitName { get; set; }
        public IEnumerable<UnitPackageAggregate> Packages { get; set; }
    }

    public class ProjectAggregate
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ProjectUnitAggregate> Units { get; set; }   
    }
}
