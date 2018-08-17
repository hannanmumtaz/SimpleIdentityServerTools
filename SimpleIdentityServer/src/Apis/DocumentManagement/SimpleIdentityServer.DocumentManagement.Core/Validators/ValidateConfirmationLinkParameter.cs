namespace SimpleIdentityServer.DocumentManagement.Core.Validators
{
    public class ValidateConfirmationLinkParameter
    {
        public string Subject { get; set; }
        public string ConfirmationCode { get; set; }
    }
}
