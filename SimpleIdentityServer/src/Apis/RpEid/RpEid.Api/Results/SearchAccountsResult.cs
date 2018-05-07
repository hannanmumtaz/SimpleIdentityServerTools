using RpEid.Api.Aggregates;
using System.Collections.Generic;

namespace RpEid.Api.Results
{
    public class SearchAccountsResult
    {
        public IEnumerable<AccountAggregate> Content { get; set; }
        public int TotalResults { get; set; }
        public int StartIndex { get; set; }
    }
}
