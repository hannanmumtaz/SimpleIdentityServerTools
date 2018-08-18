using System;

namespace SimpleIdentityServer.DocumentManagement.Store
{
    public class OfficeDocumentConfirmationLink
    {
        public string ConfirmationCode { get; set; }
        public string DocumentId { get; set; }
        public int? ExpiresIn { get; set; }
        public int? NumberOfConfirmations { get; set; }
        public DateTime CreateDateTime { get; set; }
        public DateTime UpdateDateTime { get; set; }
    }
}
