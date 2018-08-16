namespace SimpleIdentityServer.DocumentManagement.Core.Exceptions
{
    public class NotAuthorizedException : BaseDocumentManagementApiException
    {
        public NotAuthorizedException()
        {

        }

        public NotAuthorizedException(string code, string message) : base(code, message)
        {

        }
    }
}
