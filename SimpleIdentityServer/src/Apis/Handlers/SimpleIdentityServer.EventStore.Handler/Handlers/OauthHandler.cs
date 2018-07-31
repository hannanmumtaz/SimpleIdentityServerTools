using SimpleBus.Core;
using SimpleIdentityServer.OAuth.Events;
using System;

namespace SimpleIdentityServer.EventStore.Handler.Handlers
{
    public class OauthHandler : IEventHandler<TokenGranted>, IEventHandler<AuthorizationGranted>
    {
        public void Handle(TokenGranted evt)
        {
            Console.WriteLine("A token has been granted");
        }

        public void Handle(AuthorizationGranted evt)
        {
            Console.WriteLine("An authorization has been granted");
        }
    }
}
