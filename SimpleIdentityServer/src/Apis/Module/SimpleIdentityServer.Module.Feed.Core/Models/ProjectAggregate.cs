using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.Core.Models
{
    public class UnitPackageAggregate
    {
        public string Library { get; set; }
        public string Version { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<string> Parameters { get; set; }
    }
        
    public class ProjectUnitAggregate
    {
        public string UnitName { get; set; }
        public IEnumerable<UnitPackageAggregate> Packages { get; set; }
    }

    public class ProjectConnectorAggregate
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> Parameters { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }

    public class ProjectTwoFactorAuthenticatorAggregate
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string Library { get; set; }
        public string Version { get; set; }
        public IEnumerable<string> Parameters { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }

    public class ProjectAggregate
    {
        public string Id { get; set; }
        public string Version { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<ProjectUnitAggregate> Units { get; set; }   
        public IEnumerable<ProjectConnectorAggregate> Connectors { get; set; }
        public IEnumerable<ProjectTwoFactorAuthenticatorAggregate> TwoFactorAuthenticators { get; set; }
    }
}
