namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class DecryptOfficeDocumentParameter
    {
        public string DocumentId { get; set; }
        public string Kid { get; set; }
        public string Credentials { get; set; }
    }
}
