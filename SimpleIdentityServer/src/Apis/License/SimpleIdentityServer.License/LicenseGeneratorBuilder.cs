using SimpleIdentityServer.License.Exceptions;
using SimpleIdentityServer.License.Helpers;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace SimpleIdentityServer.License
{
    public interface ILicenseGeneratorBuilder
    {
        ILicenseGeneratorBuilder New();
        ILicenseGeneratorBuilder SetOrganisation(string organisation);
        ILicenseGeneratorBuilder SetType(string type);
        ILicenseGeneratorBuilder SetIssueDateTime(DateTime dateTime);
        ILicenseGeneratorBuilder SetExpirationDateTime(DateTime dateTime);
        string Build();
        void Save();
    }

    public class LicenseGeneratorBuilder : ILicenseGeneratorBuilder
    {
        private const string _pfxFile = "certificate_prk.pfx";
        private string _environment;
        private LicenseFile _licenseFile;

        public LicenseGeneratorBuilder()
        {
        }

        /// <summary>
        /// Construct a new license file.
        /// </summary>
        /// <returns></returns>
        public ILicenseGeneratorBuilder New()
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

            _environment = environment;
            _licenseFile = new LicenseFile();
            return this;
        }

        /// <summary>
        /// Set an organisation.
        /// </summary>
        /// <param name="organisation"></param>
        /// <returns></returns>
        public ILicenseGeneratorBuilder SetOrganisation(string organisation)
        {
            if (string.IsNullOrWhiteSpace(organisation))
            {
                throw new ArgumentNullException(nameof(organisation));
            }

            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            _licenseFile.Organisation = organisation;
            return this;
        }

        /// <summary>
        /// Set the type (trial / commercial).
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ILicenseGeneratorBuilder SetType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentNullException(nameof(type));
            }
            
            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            _licenseFile.Type = type;
            return this;
        }

        /// <summary>
        /// Set the issue datetime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ILicenseGeneratorBuilder SetIssueDateTime(DateTime dateTime)
        {
            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            _licenseFile.IssueDateTime = dateTime;
            return this;
        }

        /// <summary>
        /// Set the expiration datetime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public ILicenseGeneratorBuilder SetExpirationDateTime(DateTime dateTime)
        {
            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            _licenseFile.ExpirationDateTime = dateTime;
            return this;
        }

        /// <summary>
        /// Returns a signed version of the file.
        /// </summary>
        /// <returns></returns>
        public string Build()
        {
            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            var x509Certificate2 = new X509Certificate2(Path.Combine(_environment, _pfxFile));
            return CertificateHelper.Sign(x509Certificate2, _licenseFile);
        }

        /// <summary>
        /// Save the license file.
        /// </summary>
        public void Save()
        {
            if (_licenseFile == null)
            {
                throw new LicenseGeneratorNotInitializedException();
            }

            var x509Certificate2 = new X509Certificate2(Path.Combine(_environment, _pfxFile));
            var licenseFile = CertificateHelper.Sign(x509Certificate2, _licenseFile);
            File.WriteAllText(Path.Combine(_environment, Constants.LICENSE_FILE_NAME), licenseFile);
        }
    }
}
