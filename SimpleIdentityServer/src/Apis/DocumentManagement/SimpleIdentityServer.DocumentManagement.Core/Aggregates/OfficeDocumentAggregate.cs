using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Aggregates
{
    public class OfficeDocumentAggregate
    {
        public OfficeDocumentAggregate()
        {

        }

        public OfficeDocumentAggregate(string id, string subject, string publicKey)
        {
            Id = id;
            Subject = subject;
            PublicKey = publicKey;
        }

        public OfficeDocumentAggregate(string id, string subject, string publicKey, string privateKey)
        {
            Id = id;
            Subject = subject;
            PublicKey = publicKey;
            PrivateKey = privateKey;
        }

        public string Id { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaPolicyId { get; set; }
        public string Subject { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
