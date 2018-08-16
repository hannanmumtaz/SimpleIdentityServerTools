using System;

namespace SimpleIdentityServer.DocumentManagement.Core.Exceptions
{
    public class BaseDocumentManagementApiException : Exception
    {
        public BaseDocumentManagementApiException()
        {

        }


        public BaseDocumentManagementApiException(string message) : base(message) { }

        public BaseDocumentManagementApiException(string code, string message) : base(message)
        {
            Code = code;
        }

        public string Code { get; private set; }
    }
}
