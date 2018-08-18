namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class ValidateConfirmationLinkParameter
    {
        public string Subject { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
