using Newtonsoft.Json.Linq;
using SimpleIdentityServer.ResourceManager.Core.Api.Scim.Actions;
using System.Threading.Tasks;

namespace SimpleIdentityServer.ResourceManager.Core.Api.Scim
{
    public interface IScimActions
    {
        Task<JArray> GetSchemas(string subject);
    }

    internal sealed class ScimActions : IScimActions
    {
        private readonly IGetSchemasAction _getSchemasAction;

        public ScimActions(IGetSchemasAction getSchemasAction)
        {
            _getSchemasAction = getSchemasAction;
        }

        public Task<JArray> GetSchemas(string subject)
        {
            return _getSchemasAction.Execute(subject);
        }
    }
}
