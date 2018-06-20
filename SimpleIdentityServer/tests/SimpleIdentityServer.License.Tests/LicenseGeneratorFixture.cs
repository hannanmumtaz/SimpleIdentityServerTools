using System.IO;
using System.Security.Cryptography.X509Certificates;
using Xunit;

namespace SimpleIdentityServer.License.Tests
{
    public class LicenseGeneratorFixture
    {
        [Fact]
        public void WhenGenerateCertificate()
        {
            var sidPfx = Path.Combine(Directory.GetCurrentDirectory(), "Certificates", "SimpleIdServer.pfx"); // contains private key
            var sidCer = Path.Combine(Directory.GetCurrentDirectory(), "Certificates", "SimpleIdServer.cer"); // contains public key
            var sidPfxCertificate = new X509Certificate2(sidPfx);
            var sidCerCertificate = new X509Certificate2(sidCer);
            var publicKey = sidCerCertificate.PublicKey;
            string s = "";
        }
    }
}
