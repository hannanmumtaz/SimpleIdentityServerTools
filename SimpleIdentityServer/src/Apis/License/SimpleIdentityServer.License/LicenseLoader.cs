using Newtonsoft.Json;
using SimpleIdentityServer.License.Exceptions;
using SimpleIdentityServer.License.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SimpleIdentityServer.License
{
    public interface ILicenseLoader
    {
        LicenseFile TryGetLicense();
    }

    public class LicenseLoader : ILicenseLoader
    {
        private const string _pfxFile = "certificate_puk.cer";

        public LicenseFile TryGetLicense()
        {
            var environment = Environment.GetEnvironmentVariable(Constants.ENV_NAME);
            if (string.IsNullOrWhiteSpace(environment))
            {
                throw new NoSidLicenseEnvironmentVariableException();
            }

            if (!Directory.Exists(environment))
            {
                throw new BadSidLicenseEnvironmentException();
            }

            var pfxFile = Path.Combine(environment, _pfxFile);
            if (!File.Exists(pfxFile))
            {
                throw new NoCertificateException();
            }

            var licenseFile = Path.Combine(environment, Constants.LICENSE_FILE_NAME);
            if (!File.Exists(licenseFile))
            {
                throw new LicenseFileNotFoundException();
            }

            var licenseContent = File.ReadAllText(licenseFile);
            var certificate = new X509Certificate2(pfxFile);
            var splittedLicenseContent = licenseContent.Split('.');
            if (!CertificateHelper.CheckSignature(certificate, splittedLicenseContent.First(), splittedLicenseContent.Last()))
            {
                throw new BadLicenseFileException();
            }

            return JsonConvert.DeserializeObject<LicenseFile>(Encoding.UTF8.GetString(Convert.FromBase64String(splittedLicenseContent.First())));
        }
    }
}
