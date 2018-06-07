using System;

namespace SimpleIdentityServer.DocumentManagement.EF.Models
{
    public class OfficeDocument
    {
        public string Id { get; set; }
        public string UmaResourceId { get; set; }
        public string PublicKey { get; set; }
        public string Subject { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
