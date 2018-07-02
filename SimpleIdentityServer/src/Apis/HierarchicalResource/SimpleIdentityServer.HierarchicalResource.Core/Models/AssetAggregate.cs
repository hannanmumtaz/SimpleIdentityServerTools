using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.HierarchicalResource.Core.Models
{
    public class AssetAggregate
    {
        public AssetAggregate()
        {
            Children = new List<AssetAggregate>();
            AuthPolicyIds = new List<string>();
        }

        public string Hash { get; set; }
        public string ResourceParentHash { get; set; }
        public string ResourceId { get; set; }
        public IEnumerable<string> AuthPolicyIds { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string MimeType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDefaultWorkingDirectory { get; set; }
        public bool IsLocked { get; set; }
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }
        public ICollection<AssetAggregate> Children { get; set; }
    }
}
