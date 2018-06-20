using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
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
            var licenseFile = new LicenseFile
            {
                Organisation = "Roche",
                IssueDateTime = DateTime.UtcNow,
                ExpirationDateTime = DateTime.UtcNow.AddDays(30),
                Type = "commercial"
            };
            var json = JsonConvert.SerializeObject(licenseFile);
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));

            // ENCODE THE LICENSE FILE INTO BASE64
            // CALCULATE THE SIGNATURE WITH THE PRIVATE KEY AND CONCATENATE THE RESULT TO THE LICENSE FILE
            // SIGN WITH THE PRIVATE KEY
            // CHECK THE SIGNATURE WITH THE PUBLIC KEY
            // 
            string s = "";
        }
    }
}
