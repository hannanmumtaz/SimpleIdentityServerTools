using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Profile.Core.Api.Profile;
using SimpleIdentityServer.Profile.Host.DTOs;
using SimpleIdentityServer.Profile.Host.Extensions;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Profile.Host.Controllers
{
    [Route(Constants.RouteNames.ProfilesController)]
    public class ProfilesController : Controller
    {
        private readonly IProfileActions _profileActions;

        public ProfilesController(IProfileActions profileActions)
        {
            _profileActions = profileActions;
        }

        [Authorize("my_profile")]
        [HttpGet(".me")]
        public async Task<IActionResult> Get()
        {
            var subject = User.GetSubject();
            try
            {
                var profile = await _profileActions.GetProfile(subject);
                if (profile == null)
                {
                    return new NotFoundResult();
                }

                return new OkObjectResult(profile.ToDto());
            }
            catch(Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        [Authorize("my_profile")]
        [HttpPut(".me")]
        public async Task<IActionResult> Update([FromBody] ProfileResponse profile)
        {
            if (profile == null)
            {
                throw new ArgumentNullException(nameof(profile));
            }

            var subject = User.GetSubject();
            try
            {
                var parameter = profile.ToParameter();
                parameter.Subject = subject;
                if (!await _profileActions.Update(parameter))
                {
                    return this.GetError(Constants.Errors.ErrUpdateProfile, HttpStatusCode.InternalServerError);
                }

                return Ok();
            }
            catch(Exception ex)
            {
                return this.GetError(ex.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
