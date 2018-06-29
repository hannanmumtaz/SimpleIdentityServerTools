using System.Runtime.Serialization;

namespace SimpleIdentityServer.HierarchicalResource.Common.Responses
{
    [DataContract]
    public class ConfigurationResponse
    {
        [DataMember(Name = Constants.ConfigurationResponseNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ElfinderEdp)]
        public string ElfinderEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.HierarchicalresourcesEdp)]
        public string HierarchicalresourcesEdp { get; set; }
    }
}