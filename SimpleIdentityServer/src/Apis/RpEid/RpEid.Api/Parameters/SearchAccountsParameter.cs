using System;
using System.Collections.Generic;

namespace RpEid.Api.Parameters
{
    public class SearchAccountsParameter
    {
        public bool IsPagingEnabled { get; set; }
        public int StartIndex { get; set; }
        public int Count { get; set; }
        public Orders Order { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }
}
