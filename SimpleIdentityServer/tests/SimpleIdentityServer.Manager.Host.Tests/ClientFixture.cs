using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Manager.Host.Tests
{
    public class ClientFixture : IClassFixture<TestManagerServerFixture>
    {
        [Fact]
        public async Task When_Search_One_Client_Then_One_Client_Is_Returned()
        {
            // TODO
        }

        private void InitializeFakeObjects()
        {

        }
    }
}
