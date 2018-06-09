using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.DocumentManagement.Api.Extensions
{
    internal static class MappingExtensions
    {
        public static OfficeDocumentResponse ToDto(this OfficeDocumentAggregate document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new OfficeDocumentResponse
            {
                Id = document.Id,
                EncAlg = (OfficeDocumentEncAlgorithms?)document.EncAlg,
                EncPassword = document.EncPassword,
                EncSalt = document.EncSalt,
                CreateDateTime = document.CreateDateTime,
                UpdateDateTime = document.UpdateDateTime
            };
        }

        public static UpdateOfficeDocumentParameter ToParameter(this UpdateOfficeDocumentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new UpdateOfficeDocumentParameter
            {
                Permissions = request == null ? new List<OfficeDocumentPermission>() : request.Permissions.Select(p => p.ToParameter())
            };
        }

        public static OfficeDocumentAggregate ToParameter(this AddOfficeDocumentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new OfficeDocumentAggregate
            {
                Id = request.Id,
                EncAlg = (EncAlgorithms?)request.EncAlg,
                EncPassword = request.EncPassword,
                EncSalt = request.EncSalt
            };
        }

        public static OfficeDocumentPermission ToParameter(this OfficeDocumentPermissionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new OfficeDocumentPermission
            {
                Scopes = request.Scopes,
                Subject = request.Subject
            };
        }
    }
}
