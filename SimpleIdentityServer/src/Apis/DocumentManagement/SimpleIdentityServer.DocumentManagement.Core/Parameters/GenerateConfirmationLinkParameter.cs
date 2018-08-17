using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class GenerateConfirmationLinkParameter
    {
        public string Subject { get; set; }
        public int? ExpiresIn { get; set; }
        public int? NumberOfConfirmations { get; set; } 
    }
}