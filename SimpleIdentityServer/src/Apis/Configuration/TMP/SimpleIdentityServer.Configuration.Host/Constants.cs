﻿#region copyright
// Copyright 2015 Habart Thierry
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

namespace SimpleIdentityServer.Configuration
{
    internal static class Constants
    {
        public static class RouteValues
        {
            public const string AuthProvider = "authproviders";
            public const string Configuration = "configuration";
            public const string Setting = "settings";
            public const string Representations = "representations";
        }

        public static class ErrorResponseNames
        {
            public const string Code = "code";
            public const string Message = "message";
        }

        public static class ConfigurationResponseNames
        {
            public const string AuthProviderEndPoint = "authprovider_endpoint";
            public const string SettingEndPoint = "setting_endpoint";
            public const string RepresentationsEndPoint = "representations_endpoint";
        }

        public static class SettingResponseNames
        {
            public const string Key = "key";
            public const string Value = "value";
        }

        public static class RepresentationResponseNames
        {
            public const string Key = "key";
            public const string AbsoluteExpiration = "abs_exp";
            public const string SlidingExpiration = "sliding_exp";
            public const string Etag = "etag";
            public const string DateTime = "datetime";
        }

        public static class GetSettingsRequestNames
        {
            public const string Ids = "ids";
        }
    }
}
