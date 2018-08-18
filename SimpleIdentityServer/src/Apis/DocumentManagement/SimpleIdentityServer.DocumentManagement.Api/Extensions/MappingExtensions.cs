using SimpleIdentityServer.DocumentManagement.Common.DTOs.Requests;
using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.Core.Parameters;
using SimpleIdentityServer.DocumentManagement.Store;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.DocumentManagement.Api.Extensions
{
    internal static class MappingExtensions
    {
        public static OfficeDocumentInvitationLinkResponse ToDto(this OfficeDocumentConfirmationLink officeDocumentConfirmationLink)
        {
            if(officeDocumentConfirmationLink == null)
            {
                throw new ArgumentNullException(nameof(officeDocumentConfirmationLink));
            }

            return new OfficeDocumentInvitationLinkResponse
            {
                ConfirmationCode = officeDocumentConfirmationLink.ConfirmationCode,
                CreateDateTime = officeDocumentConfirmationLink.CreateDateTime,
                DocumentId = officeDocumentConfirmationLink.DocumentId,
                ExpiresIn = officeDocumentConfirmationLink.ExpiresIn,
                NumberOfConfirmations = officeDocumentConfirmationLink.NumberOfConfirmations,
                UpdateDateTime = officeDocumentConfirmationLink.UpdateDateTime
            };
        }

        public static IEnumerable<OfficeDocumentInvitationLinkResponse> ToDtos(this IEnumerable<OfficeDocumentConfirmationLink> officeDocumentConfirmationLinks)
        {
            return officeDocumentConfirmationLinks.Select(o => o.ToDto());
        }

        public static GenerateConfirmationLinkParameter ToParameter(this GenerateConfirmationCodeRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new GenerateConfirmationLinkParameter
            {
                ExpiresIn = request.ExpiresIn,
                NumberOfConfirmations = request.NumberOfConfirmations
            };
        }

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
