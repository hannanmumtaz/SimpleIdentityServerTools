using SimpleIdentityServer.HierarchicalResource.Core.Models;
using SimpleIdentityServer.ResourceManager.Common.Responses;
using System;

namespace SimpleIdentityServer.HierarchicalResource.Host.Extensions
{
    internal static class MappingExtensions
    {
        public static AssetResponse ToDto(this AssetAggregate asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            return new AssetResponse
            {
                CanRead = asset.CanRead,
                CanWrite = asset.CanWrite,
                CreatedAt = asset.CreatedAt,
                Hash = asset.Hash,
                IsDefaultWorkingDirectory = asset.IsDefaultWorkingDirectory,
                IsLocked = asset.IsLocked,
                MimeType = asset.MimeType,
                Name = asset.Name,
                Path = asset.Path,
                ResourceId = asset.ResourceId,
                ResourceParentHash = asset.ResourceParentHash
            };
        }
    }
}
