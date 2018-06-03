using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Params
{
    public class UpdateParameter
    {
        public string UnitName { get; set; }
        public string CategoryName { get; set; }
        public string LibraryName { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
    }
}
