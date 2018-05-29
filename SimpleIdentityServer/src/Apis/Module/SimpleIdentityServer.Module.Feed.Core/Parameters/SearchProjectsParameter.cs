using System.Collections.Generic;

namespace SimpleIdentityServer.Module.Feed.Core.Parameters
{
    public class SearchProjectsParameter
    {
        public SearchProjectsParameter()
        {
            ProjectNames = new List<string>();
        }

        public IEnumerable<string> ProjectNames { get; set; }
    }
}
