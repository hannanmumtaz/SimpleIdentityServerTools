using System;
using System.Runtime.Serialization;

namespace SimpleIdentityServer.ResourceManager.Common.Responses
{
    [DataContract]
    public class AssetResponse
    {
        [DataMember(Name = Constants.AssetResponseNames.Hash)]
        public string Hash { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.ResourceParentHash)]
        public string ResourceParentHash { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.ResourceId)]
        public string ResourceId { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.Name)]
        public string Name { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.Path)]
        public string Path { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.MimeType)]
        public string MimeType { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.CreatedAt)]
        public DateTime CreatedAt { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.IsDefaultWorkingDirectory)]
        public bool IsDefaultWorkingDirectory { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.IsLocked)]
        public bool IsLocked { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.CanWrite)]
        public bool CanWrite { get; set; }
        [DataMember(Name = Constants.AssetResponseNames.CanRead)]
        public bool CanRead { get; set; }
    }
}
