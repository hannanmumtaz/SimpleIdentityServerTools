using System.Threading.Tasks;
using Xunit;

namespace SimpleIdentityServer.Rms.Client.Tests
{
    public class RmsClientFixture
    {
        [Fact]
        public async Task WhenAcquireTemplateInformationThenInformationAreReturned()
        {
            // ARRANGE
            var factory = new RmsClientFactory();
            var rmsClient = factory.GetRmsClient();

            // ACT
            var result = await rmsClient.GetLicensing().GetAcquireTemplateInformation("https://543846ac-8d31-475f-8bd7-693a19e6fb60.rms.eu.aadrm.com");

            // ASSERT
            Assert.NotNull(result);
        }
    }
}
