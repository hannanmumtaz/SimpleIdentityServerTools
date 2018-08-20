namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class DeleteConfirmationCodeParameter
    {
        public string ConfirmationCode { get; set; }
        public string Subject { get; set; }
    }
}
