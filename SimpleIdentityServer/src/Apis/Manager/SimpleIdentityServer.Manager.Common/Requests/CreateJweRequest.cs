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

using Newtonsoft.Json;
using SimpleIdentityServer.Core.Common;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.Manager.Common.Requests
{
    [DataContract]
    public class CreateJweRequest
    {
        [JsonProperty(Constants.CreateJweRequestNames.Jws)]
        [DataMember(Name = Constants.CreateJweRequestNames.Jws)]
        public string Jws { get; set; }

        [JsonProperty(Constants.CreateJweRequestNames.Url)]
        [DataMember(Name = Constants.CreateJweRequestNames.Url)]
        public string Url { get; set; }

        [JsonProperty(Constants.CreateJweRequestNames.Kid)]
        [DataMember(Name = Constants.CreateJweRequestNames.Kid)]
        public string Kid { get; set; }

        [JsonProperty(Constants.CreateJweRequestNames.Alg)]
        [DataMember(Name = Constants.CreateJweRequestNames.Alg)]
        public JweAlg Alg { get; set; }

        [JsonProperty(Constants.CreateJweRequestNames.Enc)]
        [DataMember(Name = Constants.CreateJweRequestNames.Enc)]
        public JweEnc Enc { get; set; }

        [JsonProperty(Constants.CreateJweRequestNames.Password)]
        [DataMember(Name = Constants.CreateJweRequestNames.Password)]
        public string Password { get; set; }
    }
}
