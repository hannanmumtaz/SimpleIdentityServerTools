using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Module.Feed.Common.Responses
{
    [DataContract]
    public class UnitPackageResponse
    {
        [DataMember(Name = Constants.UnitPackageResponseNames.Library)]
        public string Library { get; set; }
        [DataMember(Name = Constants.UnitPackageResponseNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.UnitPackageResponseNames.CategoryName)]
        public string CategoryName { get; set; }
        [DataMember(Name = Constants.UnitPackageResponseNames.Parameters)]
        public IDictionary<string, string> Parameters { get; set; }
    }

    [DataContract]
    public class ProjectUnitResponse
    {
        [DataMember(Name = Constants.ProjectUnitResponseNames.UnitName)]
        public string UnitName { get; set; }
        [DataMember(Name = Constants.ProjectUnitResponseNames.Packages)]
        public IEnumerable<UnitPackageResponse> Packages { get; set; }
    }

    [DataContract]
    public class ProjectConnectorResponse
    {
        [DataMember(Name = Constants.ConnectorResponseNames.Name)]
        public string Name { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Description)]
        public string Description { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Picture)]
        public string Picture { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Library)]
        public string Library { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Parameters)]
        public IDictionary<string, string> Parameters { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.CreateDateTime)]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.UpdateDateTime)]
        public DateTime UpdateDateTime { get; set; }
    }

    public class ProjectTwoFactorAuthenticator
    {
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Name)]
        public string Name { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Description)]
        public string Description { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Picture)]
        public string Picture { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Library)]
        public string Library { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.Parameters)]
        public IDictionary<string, string> Parameters { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.CreateDateTime)]
        public DateTime CreateDateTime { get; set; }
        [DataMember(Name = Constants.ProjectTwoFactorAuthenticatorNames.UpdateDateTime)]
        public DateTime UpdateDateTime { get; set; }
    }

    [DataContract]
    public class ProjectResponse
    {
        [DataMember(Name = Constants.ProjectResponseNames.Id)]
        public string Id { get; set; }
        [DataMember(Name = Constants.ProjectResponseNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.ProjectResponseNames.ProjectName)]
        public string ProjectName { get; set; }
        [DataMember(Name = Constants.ProjectResponseNames.Units)]
        public IEnumerable<ProjectUnitResponse> Units { get; set; }
        [DataMember(Name = Constants.ProjectResponseNames.Connectors)]
        public IEnumerable<ProjectConnectorResponse> Connectors { get; set; }
        [DataMember(Name = Constants.ProjectResponseNames.TwoFactors)]
        public IEnumerable<ProjectTwoFactorAuthenticator> TwoFactors { get; set; }
    }
}
