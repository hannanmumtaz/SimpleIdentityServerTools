namespace SimpleIdentityServer.DocumentManagement.Core.Exceptions
{
    public class InternalDocumentException : BaseDocumentManagementApiException
    {
        public InternalDocumentException(string code, string message) : base(code, message) { }
    }
}
