using System.Collections.Generic;

namespace SimpleIdentityServer.DocumentManagement.Core.Parameters
{
    public class OfficeDocumentPermission
    {
        public string Subject { get; set; }
        public IEnumerable<string> Scopes { get; set; }
    }

    public class UpdateOfficeDocumentParameter
    {
        public UpdateOfficeDocumentParameter()
        {
            Permissions = new List<OfficeDocumentPermission>();
        }

        public string Subject { get; set; }
        public IEnumerable<OfficeDocumentPermission> Permissions { get; set; }
    }
}
