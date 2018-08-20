using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Aggregates
{
    public class OfficeDocumentAggregate
    {
        public OfficeDocumentAggregate()
        {

        }

        public OfficeDocumentAggregate(string id, string subject)
        {
            Id = id;
            Subject = subject;
        }

        public string Id { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaPolicyId { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
