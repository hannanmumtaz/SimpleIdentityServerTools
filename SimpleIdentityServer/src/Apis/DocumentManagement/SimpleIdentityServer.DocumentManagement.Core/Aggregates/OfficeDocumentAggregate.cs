using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Aggregates
{
    public enum EncAlgorithms
    {
        AES
    }
    public class OfficeDocumentAggregate
    {
        public OfficeDocumentAggregate()
        {

        }

        public OfficeDocumentAggregate(string id, string subject, EncAlgorithms? encAlg, string password, string salt)
        {
            Id = id;
            Subject = subject;
            EncAlg = encAlg;
            EncPassword = password;
            EncSalt = salt;
        }

        public string Id { get; set; }
        public EncAlgorithms? EncAlg { get; set; }
        public string EncPassword { get; set; }
        public string EncSalt { get; set; }
        public string UmaResourceId { get; set; }
        public string UmaPolicyId { get; set; }
        public string Subject { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
