using Microsoft.AspNetCore.SignalR;
using SimpleIdentityServer.Eid.UI.Stores;

namespace SimpleIdentityServer.Eid.UI.Hubs
{
    public class SessionHub : Hub
    {
        private readonly ISessionStore _sessionStore;
        private readonly IUiStore _uiStore;

        public SessionHub(ISessionStore sessionStore, IUiStore uiStore)
        {
            _sessionStore = sessionStore;
            _uiStore = uiStore;
        }

        /// <summary>
        /// Poll this function to retrieve the session.
        /// </summary>
        public void GetSession()
        {
            var session = _sessionStore.GetSession();
            if (session == null)
            {
                _uiStore.Show(); // Display the UI if the session exists.
                return;
            }
            
            Clients.All.SendAsync("Session", new { xml = session.Xml.OuterXml }); // Returns the SAML token to the client.
        }
    }
}
