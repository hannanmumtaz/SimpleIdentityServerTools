using SimpleIdentityServer.DocumentManagement.Core.Aggregates;
using System.Collections.Generic;

namespace SimpleIdentityServer.DocumentManagement.Core.Results
{
    public class SearchOfficeDocumentsResult
    {
        public IEnumerable<OfficeDocumentAggregate> Content { get; set; }
        public int Count { get; set; }
    }
}
