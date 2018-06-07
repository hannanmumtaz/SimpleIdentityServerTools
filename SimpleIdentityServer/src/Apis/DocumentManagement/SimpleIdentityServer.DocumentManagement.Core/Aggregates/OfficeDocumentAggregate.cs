using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Aggregates
{
    public class OfficeDocumentAggregate
    {
        public string Id { get; set; }
        public string PublicKey { get; set; }
        public string UmaResourceId { get; set; }
        public string Subject { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
