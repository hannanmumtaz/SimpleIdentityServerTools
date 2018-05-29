using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.EF.Models
{
    public class PackageCategory
    {
        public string Name { get; set; }
        public virtual ICollection<UnitPackage> Packages { get; set; }
    }
}
