﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleIdentityServer.Common.Dtos.Responses;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Core;
using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Api.Controllers
{
    [Route(Constants.RouteNames.OfficeDocuments)]
    public class OfficeDocumentsController : Controller
    {
        private readonly IOfficeDocumentActions _officeDocumentActions;
        private readonly DocumentManagementApiOptions _options;

        public OfficeDocumentsController(IOfficeDocumentActions officeDocumentActions, DocumentManagementApiOptions options)
        {
            _officeDocumentActions = officeDocumentActions;
            _options = options;
        }

        #region Operations

        [HttpPut("{id}")]
        [Authorize("connected")]
        public async Task<IActionResult> Update(string id, [FromBody] UpdateOfficeDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return GetError(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "id"), HttpStatusCode.BadRequest);
            }

            if (request == null)
            {
                return GetError(ErrorCodes.InvalidRequest, ErrorDescriptions.NoRequest, HttpStatusCode.BadRequest);
            }

            var subject = GetSubject();
            try
            {
                var parameter = request.ToParameter(subject);
                await _officeDocumentActions.Update(_options.OpenIdWellKnownConfiguration, id, parameter, GetAuthenticateParameter(_options));
                return new OkResult();
            }
            catch(NotAuthorizedException ex)
            {
                return GetError(ex.Code, ex.Message, HttpStatusCode.Unauthorized);
            }
        }

        [HttpPost]
        [Authorize("connected")]
        public async Task<IActionResult> Add([FromBody] AddOfficeDocumentRequest request)
        {
            if (request == null)
            {
                return GetError(ErrorCodes.InvalidRequest, ErrorDescriptions.NoRequest, HttpStatusCode.BadRequest);
            }

            var subject = GetSubject();
            var parameter = request.ToParameter(subject);
            await _officeDocumentActions.Add(_options.OpenIdWellKnownConfiguration, parameter, GetAuthenticateParameter(_options));
            return new OkResult();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return GetError(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "id"), HttpStatusCode.BadRequest);
            }

            string accessToken;
            TryGetAccessToken(out accessToken);
            try
            {
                var result = await _officeDocumentActions.Get(id, accessToken, GetAuthenticateParameter(_options));
                return new OkObjectResult(result.ToDto());
            }
            catch(NoUmaAccessTokenException ex)
            {
                Response.Headers.Add("UmaResource", ex.UmaResourceId);
                Response.Headers.Add("UmaWellKnownUrl", ex.WellKnownConfiguration);
                return GetError(ex.Code, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch(NotAuthorizedException ex)
            {
                return GetError(ex.Code, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch(DocumentNotFoundException)
            {
                return GetError(ErrorCodes.InvalidRequest, "the document doesn't exist", HttpStatusCode.NotFound);
            }
        }

        [HttpGet("permissions/{id}")]
        public async Task<IActionResult> GetPermissions(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return GetError(ErrorCodes.InvalidRequest, string.Format(ErrorDescriptions.ParameterIsMissing, "id"), HttpStatusCode.BadRequest);
            }

            try
            {

            }
            catch()
            {

            }
        }

        [HttpPost("decrypt")]
        public async Task<IActionResult> Decrypt([FromBody] DecryptDocumentRequest request)
        {
            if (request == null)
            {
                return GetError(ErrorCodes.InvalidRequest, ErrorDescriptions.NoRequest, HttpStatusCode.BadRequest);
            }

            var parameter = request.ToParameter();
            string accessToken;
            TryGetAccessToken(out accessToken);
            try
            {
                var result = await _officeDocumentActions.Decrypt(parameter, accessToken, GetAuthenticateParameter(_options));
                return new OkObjectResult(result);
            }
            catch (NoUmaAccessTokenException ex)
            {
                Response.Headers.Add("UmaResource", ex.UmaResourceId);
                Response.Headers.Add("UmaWellKnownUrl", ex.WellKnownConfiguration);
                return GetError(ex.Code, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch (NotAuthorizedException ex)
            {
                return GetError(ex.Code, ex.Message, HttpStatusCode.Unauthorized);
            }
            catch (DocumentNotFoundException)
            {
                return GetError(ErrorCodes.InvalidRequest, "the document doesn't exist", HttpStatusCode.NotFound);
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
            if (splittedAuthValue.Count() != 2 || splittedAuthValue[0] != "Bearer")
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
            
            var subjectClaim = User.Claims.FirstOrDefault(c => c.Type == "sub");
            if (subjectClaim == null)
            {
                return null;
            }

            return subjectClaim.Value;
        }

        private static IActionResult GetError(string code, string message, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError)
        {
            var error = new ErrorResponse
            {
                Error = code,
                ErrorDescription = message
            };
            return new JsonResult(error)
            {
                StatusCode = (int)httpStatusCode
            };
        }

        private static AuthenticateParameter GetAuthenticateParameter(DocumentManagementApiOptions options)
        {
            return new AuthenticateParameter
            {
                ClientId = options.OAuth.ClientId,
                ClientSecret = options.OAuth.ClientSecret,
                WellKnownConfigurationUrl = options.OAuth.WellKnownConfiguration
            };
        }
    }
}
