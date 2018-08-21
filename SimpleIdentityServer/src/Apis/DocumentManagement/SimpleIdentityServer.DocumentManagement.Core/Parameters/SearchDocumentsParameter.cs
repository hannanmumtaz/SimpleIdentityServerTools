namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class SearchDocumentsParameter
    {
        public string Subject { get; set; }
        public int StartIndex { get; set; }
        public int Count { get; set; }
    }
}
