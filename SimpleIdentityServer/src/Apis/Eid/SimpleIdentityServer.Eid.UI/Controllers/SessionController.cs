using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using PcscDotNet;
using SimpleIdentityServer.Common.Saml.Serializers;
using SimpleIdentityServer.Eid.Ehealth.Builders;
using SimpleIdentityServer.Eid.Ehealth.Tlv;
using SimpleIdentityServer.Eid.Exceptions;
using SimpleIdentityServer.Eid.Sign;
using SimpleIdentityServer.Eid.UI.Extensions;
using SimpleIdentityServer.Eid.UI.Helpers;
using SimpleIdentityServer.Eid.UI.Hubs;
using SimpleIdentityServer.Eid.UI.Stores;
using System;
using System.Linq;

namespace SimpleIdentityServer.Eid.UI.Controllers
{
    public sealed class SessionController : Controller
    {
        private readonly ISessionStore _sessionStore;
        private readonly IConfiguration _configuration;
        private readonly IEhealthSamlTokenRequestBuilder _ehealthSamlTokenRequestBuilder;
        private readonly ISoapMessageSerializer _soapMessageSerializer;
        private readonly ITlvParser _tlvParser;
        private readonly IHubContext<SessionHub> _sessionHubContext;

        public SessionController(ISessionStore sessionStore, IConfiguration configuration, IHubContext<SessionHub> sessionHubContext)
        {
            _sessionStore = sessionStore;
            _configuration = configuration;
            _ehealthSamlTokenRequestBuilder = new EhealthSamlTokenRequestBuilder();
            _soapMessageSerializer = new SoapMessageSerializer();
            _tlvParser = new TlvParser();
            _sessionHubContext = sessionHubContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var session = _sessionStore.GetSession();
            if (session == null)
            {
                return new NotFoundResult();
            }

            var timeStamp = _soapMessageSerializer.ExtractSoapTimestamp(session.Xml);
            if (timeStamp == null)
            {
                _sessionStore.StoreSession((Session)null);
                return this.BuildError(Constants.ErrorCodes.Server, Constants.ErrorMessages.InvalidSession);
            }

            return new JsonResult(new { created = timeStamp.Created, expires = timeStamp.Expires });
        }

        [HttpDelete]
        public IActionResult Remove()
        {
            _sessionStore.StoreSession((Session)null);
            return new OkResult();
        }

        [HttpPost(Constants.ActionNames.Session)]
        public IActionResult Index([FromBody] JObject json)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json));
            }

            var session = _sessionStore.GetSession();
            if (session != null)
            {
                return this.BuildError(Constants.ErrorCodes.Request, Constants.ErrorMessages.ActiveSession);
            }

            JToken jTokenPinCode;
            if (!json.TryGetValue(Constants.DtoPropertyNames.PinCode, out jTokenPinCode))
            {
                return this.BuildError(Constants.ErrorCodes.Request, Constants.ErrorMessages.NoPinCode);
            }
            
            if (ConfigurationHelper.IsFakeEidEnabled()) // For testing purpose we generate a fake session.
            {
                /*
                var payload = System.Convert.FromBase64String("MIID7zCCAtegAwIBAgIQEAAAAAAAjzo9FBi9BAvSgTANBgkqhkiG9w0BAQUFADA1MQswCQYDVQQGEwJCRTEVMBMGA1UEAxMMRm9yZWlnbmVyIENBMQ8wDQYDVQQFEwYyMDE0MDIwHhcNMTQwMjA1MTIzMzEyWhcNMTkwMTMwMjM1OTU5WjB3MQswCQYDVQQGEwJGUjEoMCYGA1UEAxMfVGhpZXJyeSBIYWJhcnQgKEF1dGhlbnRpY2F0aW9uKTEPMA0GA1UEBBMGSGFiYXJ0MRcwFQYDVQQqEw5UaGllcnJ5IFJvYmVydDEUMBIGA1UEBRMLODkxMDA3Mzk1NzMwgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAI4At/6KxOpRKiFEluuMVRaP+YI4gKKeVpl10CY+vPln9P3dvUh7gdNWfEN2Fn0LzvOslVDWZeb+A8/HBhPePpTJ9sACPJG+BjcLKZYKwjc6eEKAHmDTqClTuvrSi5hzpEuDJ7gHQmzJcochdcKRr06MIg3wP7P8s91VxSJoKJ0/AgMBAAGjggE7MIIBNzAfBgNVHSMEGDAWgBTD/rhgVgSUYRHFYhTWJlYgS27SvTBwBggrBgEFBQcBAQRkMGIwNgYIKwYBBQUHMAKGKmh0dHA6Ly9jZXJ0cy5laWQuYmVsZ2l1bS5iZS9iZWxnaXVtcnMyLmNydDAoBggrBgEFBQcwAYYcaHR0cDovL29jc3AuZWlkLmJlbGdpdW0uYmUvMjBEBgNVHSAEPTA7MDkGB2A4CQEBBwIwLjAsBggrBgEFBQcCARYgaHR0cDovL3JlcG9zaXRvcnkuZWlkLmJlbGdpdW0uYmUwOQYDVR0fBDIwMDAuoCygKoYoaHR0cDovL2NybC5laWQuYmVsZ2l1bS5iZS9laWRmMjAxNDAyLmNybDAOBgNVHQ8BAf8EBAMCB4AwEQYJYIZIAYb4QgEBBAQDAgWgMA0GCSqGSIb3DQEBBQUAA4IBAQBSz7layb5HgSQM+ZB2np1/gAUJ+8wuxoU62vxGrj7T1K5tmRK5WeX4rpEJ8h0Nv1169GGng77xlAdE9s8noQXQ063SuBg/QAXSYTRF2T+g9rZinC12aljo/bLz8jWSBY8AM4GZQcVAQ31aD+mWzO0DPwVtDvhl0/6jFVprKFDhAnPJvzMQe7a6zwi5lL+GXIKrnNhlvfQDtxJRV4Jr1vps7ZBjXnAOuatMIxlJ9eafU3dgQ2TvTpwSVDV8UgrInhKD9cLBzymli4LeMWyD6qC3SwfDwRVODqNSIogBnSH91fn3nZUDKpUYfOCSGxNozGYZsVnqwO0jJf4E69O5lUgg");
                var certificate = new X509Certificate(payload);
                var soapEnvelope = _ehealthSamlTokenRequestBuilder.New(certificate).Build();
                var xmlDocument = _soapMessageSerializer.Serialize(soapEnvelope); // STORE THE SOAP REQUEST.
                _sessionStore.StoreSession(xmlDocument, type);
                */
            }
            else
            {
                BeIdCardConnector beIdCardConnector = null;
                PcscConnection connect = null;
                PcscContext context = null;
                try
                {
                    beIdCardConnector = new BeIdCardConnector(); // 1. Try to connect to the card reader.
                    context = beIdCardConnector.EstablishContext();
                    var readers = beIdCardConnector.GetReaders();
                    if (!readers.Any())
                    {
                        return this.BuildError(Constants.ErrorCodes.Eid, Constants.ErrorMessages.NoCard);
                    }

                    connect = beIdCardConnector.Connect(readers.First()); // 2. Construct SAML token.
                    var certificate = beIdCardConnector.GetAuthenticateCertificate();
                    var identityPayload = beIdCardConnector.GetIdentity();
                    var addressPayload = beIdCardConnector.GetAddress();
                    var picturePayload = beIdCardConnector.GetPhoto();
                    var identity = _tlvParser.Parse<Identity>(identityPayload);
                    var address = _tlvParser.Parse<Address>(addressPayload);

                    var builder = _ehealthSamlTokenRequestBuilder.New(certificate);
                    var soapEnvelope = builder.SetImage(picturePayload).SetIdentity(identity).SetAddress(address).Build();
                    var signSamlToken = new SignSamlToken();
                    var signatureNode = signSamlToken.BuildSignatureWithEid(soapEnvelope, jTokenPinCode.ToString(), beIdCardConnector); // 3. Build signature.
                    soapEnvelope.Header.Security.Signature = signatureNode;
                    var xmlDocument = _soapMessageSerializer.Serialize(soapEnvelope);
                    _sessionStore.StoreSession(xmlDocument);
                    _sessionHubContext.Clients.All.SendAsync("Session", new { xml = xmlDocument.OuterXml });
                }
                catch (BeIdCardException ex)
                {
                    return this.BuildError(Constants.ErrorCodes.Eid, ex.Message);
                }
                catch (Exception e)
                {
                    return this.BuildError(Constants.ErrorCodes.Server, Constants.ErrorMessages.CardError);
                }
                finally
                {
                    beIdCardConnector.Dispose();
                }
            }

            return new OkResult();
        }
    }
}
