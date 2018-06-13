namespace SimpleIdentityServer.Module.Feed.Common
{
    internal static class Constants
    {
        public static class ProjectResponseNames
        {
            public const string Id = "id";
            public const string Version = "version";
            public const string ProjectName = "name";
            public const string Units = "units";
            public const string Connectors = "connectors";
        }

        public static class ProjectUnitResponseNames
        {
            public const string UnitName = "name";
            public const string Packages = "packages";
        }

        public static class UnitPackageResponseNames
        {
            public const string Library = "lib";
            public const string Version = "version";
            public const string CategoryName = "category";
            public const string Parameters = "parameters";
        }

        public static class ConnectorResponseNames
        {
            public const string Name = "name";
            public const string Description = "description";
            public const string Picture = "picture";
            public const string Library = "lib";
            public const string Version = "version";
            public const string Parameters = "parameters";
            public const string CreateDateTime = "create_datetime";
            public const string UpdateDateTime = "update_datetime";
        }
    }
}
