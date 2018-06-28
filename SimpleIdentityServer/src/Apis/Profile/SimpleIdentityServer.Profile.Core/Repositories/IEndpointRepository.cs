using SimpleIdentityServer.Profile.Core.Models;
using SimpleIdentityServer.Profile.Core.Parameters;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Profile.Core.Repositories
{
    public interface IEndpointRepository
    {
        Task<EndpointAggregate> Get(string url);
        Task<bool> Remove(string url);
        Task<bool> Add(IEnumerable<EndpointAggregate> idProviders);
        Task<IEnumerable<EndpointAggregate>> GetAll();
        Task<IEnumerable<EndpointAggregate>> Search(SearchEndpointsParameter parameter);
    }
}
