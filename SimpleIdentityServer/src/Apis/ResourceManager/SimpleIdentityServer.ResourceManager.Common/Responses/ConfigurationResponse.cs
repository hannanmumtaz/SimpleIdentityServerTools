using System.Runtime.Serialization;

namespace SimpleIdentityServer.ResourceManager.Common.Responses
{
    [DataContract]
    public class ConfigurationResponse
    {
        [DataMember(Name = Constants.ConfigurationResponseNames.Version)]
        public string Version { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.EndpointsEdp)]
        public string EndpointsEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ProfileEdp)]
        public string ProfileEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ElfinderEdp)]
        public string ElfinderEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ClientsEdp)]
        public string ClientsEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ScopesEdp)]
        public string ScopesEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ResourceOwnersEdp)]
        public string ResourceOwnersEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ClaimsEdp)]
        public string ClaimsEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ResourcesEdp)]
        public string ResourcesEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.AuthPoliciesEdp)]
        public string AuthPoliciesEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ScimEdp)]
        public string ScimEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.HierarchicalresourcesEdp)]
        public string HierarchicalresourcesEdp { get; set; }
        [DataMember(Name = Constants.ConfigurationResponseNames.ParametersEdp)]
        public string ParametersEdp { get; set; }
    }
}