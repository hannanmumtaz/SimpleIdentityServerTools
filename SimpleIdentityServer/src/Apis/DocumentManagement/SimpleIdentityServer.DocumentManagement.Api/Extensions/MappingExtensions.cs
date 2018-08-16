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
                CreateDateTime = document.CreateDateTime,
                UpdateDateTime = document.UpdateDateTime
            };
        }

        public static UpdateOfficeDocumentParameter ToParameter(this UpdateOfficeDocumentRequest request, string subject)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new UpdateOfficeDocumentParameter
            {
                Subject = subject,
                Permissions = request == null ? new List<OfficeDocumentPermission>() : request.Permissions.Select(p => p.ToParameter())
            };
        }

        public static DecryptOfficeDocumentParameter ToParameter(this DecryptDocumentRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new DecryptOfficeDocumentParameter
            {
                Credentials = request.Credentials,
                DocumentId = request.DocumentId,
                Kid = request.Kid
            };
        }

        public static AddDocumentParameter ToParameter(this AddOfficeDocumentRequest request, string subject)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new AddDocumentParameter
            {
                Id = request.Id,
                Subject = subject
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
