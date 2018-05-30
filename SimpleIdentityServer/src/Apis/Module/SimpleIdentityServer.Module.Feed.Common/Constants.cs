﻿namespace SimpleIdentityServer.Module.Feed.Common
{
    internal static class Constants
    {
        public static class ProjectResponseNames
        {
            public const string Id = "id";
            public const string Version = "version";
            public const string ProjectName = "name";
            public const string Units = "units";
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
        }
    }
}