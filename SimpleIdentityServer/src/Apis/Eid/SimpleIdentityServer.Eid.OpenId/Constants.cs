#region copyright
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

namespace SimpleIdentityServer.Eid.OpenId
{
    public static class Constants
    {
        public const string CookieName = "SimpleIdServer-Startup";
        public const string ExternalCookieName = "External-SimpleIdServer";

        public static class LoginViewModelNames
        {
            public const string Xml = "xml";
            public const string IdProviders = "id_providers";
        }

        public static class EidAuthorizeViewModelNames
        {
            public const string Code = "code";
            public const string Xml = "xml";
            public const string IdProviders = "id_providers";
        }
    }
}
