using SimpleIdentityServer.DocumentManagement.Common.DTOs.Responses;
using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.EF.Models;
using System;

namespace SimpleIdentityServer.DocumentManagement.EF.Extensions
{
    internal static class MappingExtensions
    {
        public static OfficeDocumentJsonWebKeyResponse ToDomain(this OfficeDocumentJsonWebKey jsonWebKey)
        {
            Uri x5u = null;
            if (Uri.IsWellFormedUriString(jsonWebKey.X5u, UriKind.Absolute))
            {
                x5u = new Uri(jsonWebKey.X5u);
            }

            return new OfficeDocumentJsonWebKeyResponse
            {
                Kid = jsonWebKey.Kid,
                Alg = (AllAlgResponse)jsonWebKey.Alg,
                Kty = (KeyTypeResponse)jsonWebKey.Kty,
                X5t = jsonWebKey.X5t,
                X5tS256 = jsonWebKey.X5tS256,
                X5u = x5u,
                SerializedKey = jsonWebKey.SerializedKey
            };
        }

        public static OfficeDocumentAggregate ToDomain(this OfficeDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new OfficeDocumentAggregate
            {
                Id = document.Id,
                Subject = document.Subject,
                DisplayName = document.DisplayName,
                UmaResourceId = document.UmaResourceId,
                UmaPolicyId = document.UmaPolicyId,
                UpdateDateTime = document.UpdateDateTime,
                CreateDateTime = document.CreateDateTime
            };
        }

        public static OfficeDocument ToModel(this OfficeDocumentAggregate document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return new OfficeDocument
            {
                Id = document.Id,
                Subject = document.Subject,
                DisplayName = document.DisplayName,
                UmaResourceId = document.UmaResourceId,
                UmaPolicyId = document.UmaPolicyId,
                UpdateDateTime = document.UpdateDateTime,
                CreateDateTime = document.CreateDateTime
            };
        }
    }
}
