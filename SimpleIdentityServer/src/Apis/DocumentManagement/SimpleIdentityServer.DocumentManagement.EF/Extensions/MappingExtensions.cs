using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using SimpleIdentityServer.DocumentManagement.EF.Models;
using System;

namespace SimpleIdentityServer.DocumentManagement.EF.Extensions
{
    internal static class MappingExtensions
    {
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
                UmaResourceId = document.UmaResourceId,
                UmaPolicyId = document.UmaPolicyId,
                UpdateDateTime = document.UpdateDateTime,
                CreateDateTime = document.CreateDateTime,
                PublicKey = document.PublicKey
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
                UmaResourceId = document.UmaResourceId,
                UmaPolicyId = document.UmaPolicyId,
                UpdateDateTime = document.UpdateDateTime,
                CreateDateTime = document.CreateDateTime,
                PublicKey = document.PublicKey
            };
        }
    }
}
