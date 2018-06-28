namespace SimpleIdentityServer.ResourceManager.Common
{
    internal static class Constants
    {
        public static class AssetResponseNames
        {
            public const string Hash = "hash";
            public const string ResourceParentHash = "parent_hash";
            public const string ResourceId = "resource_id";
            public const string Name = "name";
            public const string Path = "path";
            public const string MimeType = "mime_type";
            public const string CreatedAt = "create_time";
            public const string IsDefaultWorkingDirectory = "default_wkd";
            public const string IsLocked = "is_locked";
            public const string CanWrite = "can_write";
            public const string CanRead = "can_read";
        }

        public static class ConfigurationResponseNames
        {
            public const string Version = "version";
            public const string EndpointsEdp = "endpoints_endpoint";
            public const string ProfileEdp = "profile_endpoint";
            public const string ElfinderEdp = "elfinder_endpoint";
            public const string ClientsEdp = "clients_endpoint";
            public const string ScopesEdp = "scopes_endpoint";
            public const string ResourceOwnersEdp = "resourceowners_endpoint";
            public const string ClaimsEdp = "claims_endpoint";
            public const string ResourcesEdp = "resources_endpoint";
            public const string AuthPoliciesEdp = "authpolicies_endpoint";
            public const string ScimEdp = "scim_endpoint";
            public const string HierarchicalresourcesEdp = "hierarchicalresources_endpoint";
            public const string ParametersEdp = "parameters_endpoint";
        }
    }
}
