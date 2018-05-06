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

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Core.WebSite.User;
using SimpleIdentityServer.Host;
using SimpleIdentityServer.Host.Controllers.Website;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Eid.OpenId.Controllers
{
    public class HomeController : BaseController
    {   
        public HomeController(IAuthenticationService authenticationService, IUserActions userActions, AuthenticateOptions authenticateOptions) 
            : base(authenticationService, userActions, authenticateOptions)
        {

        }

        #region Public methods

        [HttpGet]
        public async Task<ActionResult> Index() 
        {
            await SetUser();
            return View();    
        }
        
        #endregion
    }
}