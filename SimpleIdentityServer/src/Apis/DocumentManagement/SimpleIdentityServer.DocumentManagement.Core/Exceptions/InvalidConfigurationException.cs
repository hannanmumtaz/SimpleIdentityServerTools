namespace SimpleIdentityServer.DocumentManagement.Core.Exceptions
{
    public class InvalidConfigurationException : BaseDocumentManagementApiException
    {
        public InvalidConfigurationException(string message) : base(message) { }
    }
}