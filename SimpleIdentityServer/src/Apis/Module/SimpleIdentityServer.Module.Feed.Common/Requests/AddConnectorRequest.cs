using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Module.Feed.Common.Requests
{
    [DataContract]
    public class AddConnectorRequest
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
        public IEnumerable<string> Parameters { get; set; }
    }
}
