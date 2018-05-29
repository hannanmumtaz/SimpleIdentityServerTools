using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class Unit
    {
        public string UnitName { get; set; }
        public virtual ICollection<UnitPackage> Packages { get; set; }
        public virtual ICollection<ProjectUnit> Projects { get; set; }
        public virtual Project Project { get; set; }
    }
}
