using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.HierarchicalResource.Client;
using SimpleIdentityServer.HierarchicalResource.Common.Responses;
using SimpleIdentityServer.Uma.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Resolver
{
    public interface IResourceManagerResolver
    {
        Task<IEnumerable<string>> ResolveAccessibleResources(string identityToken);
        Task UpdateViewBag(Controller controller);
    }

    internal sealed class ResourceManagerResolver : IResourceManagerResolver
    {
        private readonly IIdentityServerClientFactory _identityServerClientFactory;
        private readonly IHierarchicalResourceClientFactory _hierarchicalResourceClientFactory;
        private readonly IIdentityServerUmaClientFactory _identityServerUmaClientFactory;
        private readonly ResourceManagerResolverOptions _resourceManagerResolverOptions;
        private readonly IDataProtector _dataProtector;

        public ResourceManagerResolver(IIdentityServerClientFactory identityServerClientFactory,
            IHierarchicalResourceClientFactory hierarchicalResourceClientFactory, IIdentityServerUmaClientFactory identityServerUmaClientFactory,
            IDataProtectionProvider dataProtectionProvider, ResourceManagerResolverOptions resourceManagerResolverOptions)
        {
            _identityServerClientFactory = identityServerClientFactory;
            _hierarchicalResourceClientFactory = hierarchicalResourceClientFactory;
            _identityServerUmaClientFactory = identityServerUmaClientFactory;
            _resourceManagerResolverOptions = resourceManagerResolverOptions;
            _dataProtector = dataProtectionProvider.CreateProtector(Constants.ProtectorName);
        }

        public async Task UpdateViewBag(Controller controller)
        {
            if (controller == null)
            {
                throw new ArgumentNullException(nameof(controller));
            }

            ICollection<string> accessibleResources;
            if (!controller.Request.Cookies.ContainsKey(Constants.DefaultCookieName))
            {
                var getHierarchicalResource = await _hierarchicalResourceClientFactory.GetHierarchicalResourceClient()
                    .Get(new Uri(_resourceManagerResolverOptions.ResourceManagerUrl), _resourceManagerResolverOptions.Url, true);
                accessibleResources = getHierarchicalResource.Content.Where(r => string.IsNullOrWhiteSpace(r.ResourceId)).Select(r => r.Path).ToList();
                var json = JsonConvert.SerializeObject(accessibleResources);
                var protect = _dataProtector.Protect(json);
                var now = DateTime.UtcNow;
                var expires = now.AddSeconds(3600);
                var cookieOptions = new CookieOptions
                {
                    Expires = expires
                };
                controller.Response.Cookies.Append(Constants.DefaultCookieName, protect, cookieOptions);
            }
            else
            {
                accessibleResources = controller.GetAccessibleResources(_dataProtector);
            }

            
            controller.ViewBag.AccessibleResources = accessibleResources;
        }

        public async Task<IEnumerable<string>> ResolveAccessibleResources(string identityToken)
        {
            if (string.IsNullOrWhiteSpace(identityToken))
            {
                throw new ArgumentNullException(nameof(identityToken));
            }

            var getHierarchicalResource = await _hierarchicalResourceClientFactory.GetHierarchicalResourceClient()
                .Get(new Uri(_resourceManagerResolverOptions.ResourceManagerUrl), _resourceManagerResolverOptions.Url, true);
            var resources = getHierarchicalResource.Content.Where(r => !string.IsNullOrWhiteSpace(r.ResourceId));
            var resourcePathLst = getHierarchicalResource.Content.Where(r => string.IsNullOrWhiteSpace(r.ResourceId)).Select(r => r.Path).ToList();
            var grantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_resourceManagerResolverOptions.Authorization.ClientId, _resourceManagerResolverOptions.Authorization.ClientSecret)
                .UseClientCredentials("uma_protection")
                .ResolveAsync(_resourceManagerResolverOptions.Authorization.AuthorizationWellKnownConfiguration);
            var tasks = new List<Task<string>>();
            foreach (var resource in resources)
            {
                tasks.Add(ResolveUrl(resource, grantedToken.Content.AccessToken, identityToken));
            }

            var grantedPathLst = (await Task.WhenAll(tasks)).Where(p => !string.IsNullOrWhiteSpace(p));
            resourcePathLst.AddRange(grantedPathLst);
            return resourcePathLst;
        }

        private async Task<string> ResolveUrl(AssetResponse asset, string accessToken, string idToken)
        {
            var permissionResponse = await _identityServerUmaClientFactory.GetPermissionClient()
                .AddByResolution(new PostPermission
                {
                    ResourceSetId = asset.ResourceId,
                    Scopes = new[]
                    {
                        "read"
                    },
                }, _resourceManagerResolverOptions.Authorization.AuthorizationWellKnownConfiguration, accessToken);
            var umaGrantedToken = await _identityServerClientFactory.CreateAuthSelector()
                .UseClientSecretPostAuth(_resourceManagerResolverOptions.Authorization.ClientId, _resourceManagerResolverOptions.Authorization.ClientSecret)
                .UseTicketId(permissionResponse.TicketId, idToken)
                .ResolveAsync(_resourceManagerResolverOptions.Authorization.AuthorizationWellKnownConfiguration);
            if (umaGrantedToken == null || string.IsNullOrWhiteSpace(umaGrantedToken.Content.AccessToken))
            {
                return null;
            }

            return asset.Path;
        }
    }
}
