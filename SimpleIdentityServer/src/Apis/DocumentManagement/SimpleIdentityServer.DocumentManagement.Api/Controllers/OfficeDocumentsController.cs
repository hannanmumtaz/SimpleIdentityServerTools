using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Api.Controllers
{
    [Route(Constants.RouteNames.OfficeDocumentsController)]
    public class OfficeDocumentsController : Controller
    {
        private readonly IOfficeDocumentActions _officeDocumentActions;
        private readonly OAuthOptions _options;

        public OfficeDocumentsController(IOfficeDocumentActions officeDocumentActions, OAuthOptions options)
        {
            _officeDocumentActions = officeDocumentActions;
            _options = options;
        }

        #region Operations

        [HttpPost("{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOfficeDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var subject = GetSubject();
            if (string.IsNullOrWhiteSpace(subject))
            {
                return new NotFoundResult();
            }

            try
            {
                var parameter = request.ToParameter();
                await _officeDocumentActions.Update(subject, id, request.ToParameter(), GetAuthenticateParameter(_options));
                return new OkResult();
            }
            catch(NotAuthorizedException)
            {
                return new UnauthorizedResult();
            }
            catch (BaseDocumentManagementApiException ex)
            {
                return GetError(ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                return GetError("internal", ex.Message);
            }
        }

        [HttpPost]
        [Authorize("connected")]
        public async Task<IActionResult> Add([FromBody] AddOfficeDocumentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var subject = GetSubject();
            if (string.IsNullOrWhiteSpace(subject))
            {
                return new NotFoundResult();
            }

            try
            {
                var parameter = request.ToParameter();
                parameter.Subject = subject;
                await _officeDocumentActions.Add(request.ToParameter(), GetAuthenticateParameter(_options));
                return new OkResult();
            }
            catch (BaseDocumentManagementApiException ex)
            {
                return GetError(ex.Code, ex.Message);
            }
            catch (Exception ex)
            {
                return GetError("internal", ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            string accessToken;
            if (!TryGetAccessToken(out accessToken))
            {
                return new UnauthorizedResult();
            }

            try
            {
                var result = await _officeDocumentActions.Get(id, accessToken, GetAuthenticateParameter(_options));
                return new OkObjectResult(result.ToDto());
            }
            catch(DocumentNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (BaseDocumentManagementApiException ex)
            {
                return GetError(ex.Code, ex.Message);
            }
            catch(Exception ex)
            {
                return GetError("internal", ex.Message);
            }
        }

        #endregion

        private bool TryGetAccessToken(out string accessToken)
        {
            accessToken = null;
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return false;
            }

            var authValue = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrWhiteSpace(authValue))
            {
                return false;
            }

            var splittedAuthValue = authValue.Split(' ');
            if (splittedAuthValue.Count() != 2 || splittedAuthValue[0] == "Bearer")
            {
                return false;
            }

            accessToken = splittedAuthValue[1];
            return false;
        }

        private string GetSubject()
        {
            if (User == null || User.Claims == null)
            {
                return null;
            }

            var identity = User.Claims as ClaimsIdentity;
            if (identity == null)
            {
                return null;
            }

            var subjectClaim = identity.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subjectClaim == null)
            {
                return null;
            }

            return subjectClaim.Value;
        }

        private static IActionResult GetError(string code, string message)
        {
            var error = new ErrorResponse
            {
                Code = code,
                Message = message
            };
            return new JsonResult(error)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError
            };
        }

        private static AuthenticateParameter GetAuthenticateParameter(OAuthOptions options)
        {
            return new AuthenticateParameter
            {
                ClientId = options.ClientId,
                ClientSecret = options.ClientSecret,
                WellKnownConfigurationUrl = options.WellKnownConfiguration
            };
        }
    }
}
