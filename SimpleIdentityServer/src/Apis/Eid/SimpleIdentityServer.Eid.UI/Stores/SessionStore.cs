using SimpleIdentityServer.Eid.Common.Serializers;
using System;
using System.Xml;

namespace SimpleIdentityServer.Eid.UI.Stores
{
    public interface ISessionStore
    {
        bool IsValid();
        Session GetSession();
        void StoreSession(Session session);
        void StoreSession(XmlDocument session, string role);
    }
    
    public class Session
    {
        public XmlDocument Xml { get; set; }
        public string Role { get; set; }
    }

    internal sealed class SessionStore : ISessionStore
    {
        private readonly ISoapMessageSerializer _soapMessageSerializer;
        private Session _session;

        public SessionStore()
        {
            _soapMessageSerializer = new SoapMessageSerializer();
        }

        public bool IsValid()
        {
            if (_session == null)
            {
                return false;
            }

            var soapTimeStamp = _soapMessageSerializer.ExtractSoapTimestamp(_session.Xml);
            if (soapTimeStamp == null)
            {
                return false;
            }

            var now = DateTime.UtcNow;
            return soapTimeStamp.Expires >= now;
        }

        public Session GetSession()
        {
            if (!IsValid())
            {
                return null;
            }

            return _session;
        }

        public void StoreSession(Session session)
        {
            _session = session;
        }

        public void StoreSession(XmlDocument session, string role)
        {
            _session = new Session { Role = role, Xml = session };
        }
    }
}
