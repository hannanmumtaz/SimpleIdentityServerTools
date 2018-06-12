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
    public class ConnectorResponse
    {
        [DataMember(Name = Constants.ConnectorResponseNames.Name)]
        public string Name { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Library)]
        public string Library { get; set; }
        [DataMember(Name = Constants.ConnectorResponseNames.Version)]
        public string Version { get; set; }
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
        public IEnumerable<ConnectorResponse> Connectors { get; set; }
    }
}
