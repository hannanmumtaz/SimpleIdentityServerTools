using SimpleIdentityServer.Eid.Common.SamlMessages;
using SimpleIdentityServer.Eid.Common.SoapMessages;
using SimpleIdentityServer.Eid.Ehealth.Exceptions;
using SimpleIdentityServer.Eid.Ehealth.Tlv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace SimpleIdentityServer.Eid.Ehealth.Builders
{
    public interface IEhealthSamlTokenRequestBuilder
    {
        EhealthSamlTokenRequestBuilder New(X509Certificate x509Certificate);
        EhealthSamlTokenRequestBuilder AddAttribute(string name, string namespaceValue, string value);
        EhealthSamlTokenRequestBuilder AddAttributeDesignator(string name, string namespaceValue);
        SoapEnvelope Build();
    }

    public class EhealthSamlTokenRequestBuilder : IEhealthSamlTokenRequestBuilder
    {
        private static Dictionary<string, string> _mappingNameToOid = new Dictionary<string, string> // Mapping according to this website : http://www.alvestrand.no/objectid/2.5.4.html
        {
            { "SERIALNUMBER=", "OID.2.5.4.5=" }, // Serial number.
            { "G=", "OID.2.5.4.42=" }, // Given name.
            { "SN=", "OID.2.5.4.4=" } // Surname
        };

        private X509Certificate _x509Certificate;
        private ICollection<SamlAttributeDesignator> _samlAttributeDesignators;
        private ICollection<SamlAttribute> _samlAttributes;

        public EhealthSamlTokenRequestBuilder()
        {
            Clean();
        }

        #region Public methods

        public EhealthSamlTokenRequestBuilder New(X509Certificate x509Certificate)
        {
            if (x509Certificate == null)
            {
                throw new ArgumentNullException(nameof(x509Certificate));
            }

            _x509Certificate = x509Certificate;
            return this;
        }

        public EhealthSamlTokenRequestBuilder AddAttribute(string name, string namespaceValue, string value)
        {
            CheckInit();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(namespaceValue))
            {
                throw new ArgumentNullException(namespaceValue);
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            _samlAttributes.Add(new SamlAttribute(name, namespaceValue, value));
            return this;
        }

        public EhealthSamlTokenRequestBuilder AddAttributeDesignator(string name, string namespaceValue)
        {
            CheckInit();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(namespaceValue))
            {
                throw new ArgumentNullException(namespaceValue);
            }

            _samlAttributeDesignators.Add(new SamlAttributeDesignator(name, namespaceValue));
            return this;
        }

        public EhealthSamlTokenRequestBuilder SetIdentity(Identity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException(nameof(identity));
            }

            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.NameAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, identity.Name));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.FirstNameAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, identity.FirstName));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.MiddleNameAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, identity.MiddleName));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.GenderAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, identity.Gender));
            return this;
        }

        public EhealthSamlTokenRequestBuilder SetAddress(Address address)
        {
            if (address == null)
            {
                throw new ArgumentException(nameof(address));
            }

            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.StreetAndNumberAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, address.StreetAndNumber));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.MunicipalityAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, address.Municipality));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.ZipAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, address.Zip));
            return this;
        }

        public EhealthSamlTokenRequestBuilder SetImage(IEnumerable<byte> payload)
        {
            if (payload == null)
            {
                throw new ArgumentException(nameof(payload));
            }

            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.PictureAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, ByteArrayToString(payload)));
            return this;
        }

        public SoapEnvelope Build()
        {
            CheckInit();
            var samlAssertionId = GenerateId("assertion");
            var requestId = GenerateId("request");
            var bodyId = GenerateId("id");
            var timeStampId = GenerateId("TS");
            var x509Id = GenerateId("X509");
            var ssin = GetSsin(_x509Certificate.Subject);
            if (string.IsNullOrWhiteSpace(ssin))
            {
                throw new EhealthException(Constants.ErrorCodes.NoSerialNumber);
            }
            
            var identitySubject = ParseSubject(_x509Certificate.Subject);
            var issuerSubject = ParseSubject(_x509Certificate.Issuer);
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.SsinCertHolderAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, ssin));
            _samlAttributes.Add(new SamlAttribute(Constants.EhealthStsNames.SsinAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace, ssin));
            _samlAttributeDesignators.Add(new SamlAttributeDesignator(Constants.EhealthStsNames.SsinCertHolderAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace));
            _samlAttributeDesignators.Add(new SamlAttributeDesignator(Constants.EhealthStsNames.SsinAttributeName, Constants.EhealthStsNames.SsinAttributeNamespace));
            var issueInstant = DateTime.Now;
            var samlNameIdentifier = new SamlNameIdentifier(
                Constants.EhealthStsNames.NameIdentifierFormat,
                issuerSubject,
                identitySubject);
            var samlSubject = new SamlSubject(samlNameIdentifier);
            var samlConditions = new SamlConditions(issueInstant);
            var samlAttributeStatement = new SamlAttributeStatement(samlSubject, _samlAttributes);
            var samlAssertion = new SamlAssertion(samlAssertionId, issueInstant, identitySubject, samlConditions, samlAttributeStatement);
            var subjectConfirmationData = new SamlSubjectConfirmationData(samlAssertion);
            var subjectConfirmation = new SamlSubjectConfirmation(Constants.EhealthStsNames.SubjectConfirmationMethod, _x509Certificate, subjectConfirmationData);
            var samlSubjectO = new SamlSubject(samlNameIdentifier, subjectConfirmation);
            var samlAttributeQuery = new SamlAttributeQuery(samlSubjectO, _samlAttributeDesignators);
            var samlRequest = new SamlRequest(requestId, samlAttributeQuery);
            var body = new SoapBody(samlRequest, bodyId);
            var soapSecurity = new SoapSecurity(DateTime.UtcNow, timeStampId, x509Id, _x509Certificate);
            var header = new SoapHeader(soapSecurity);
            var soapEnvelope = new SoapEnvelope(header, body);
            return soapEnvelope;
        }
        
        #endregion

        #region Private methods

        private void Clean()
        {
            _samlAttributeDesignators = new List<SamlAttributeDesignator>();
            _samlAttributes = new List<SamlAttribute>();
            _x509Certificate = null;
        }

        private void CheckInit()
        {
            if (_x509Certificate == null)
            {
                throw new EhealthException(Constants.ErrorCodes.NotInit);
            }
        }

        #endregion

        private static string GetSsin(string subject)
        {
            var regex = new Regex(@"SERIALNUMBER=(\d|\s)*");
            var matchResult = regex.Matches(subject);
            if (matchResult.Count != 1)
            {
                return null;
            }

            return matchResult[0].Value.Split('=').Last();
        }

        private static string ParseSubject(string subject)
        {
            foreach (var kvp in _mappingNameToOid)
            {
                subject = subject.Replace(kvp.Key, kvp.Value);
            }

            return subject;
        }

        public static string GenerateId(string prefix)
        {
            return $"{prefix}-{Guid.NewGuid().ToString()}".ToLower();
        }

        private static string ByteArrayToString(IEnumerable<byte> ba)
        {
            var hex = new StringBuilder(ba.Count() * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }
    }
}
