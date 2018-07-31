using SimpleBus.Core;
using SimpleBus.InMemory;
using SimpleIdentityServer.EventStore.Handler.Handlers;
using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.EventStore.Handler
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var handlers = new List<IEventHandler>
            {
                new OauthHandler()
            };
            var eventSubscriber = new InMemoryEventSubscriber(new InMemoryOptions(), handlers);
            eventSubscriber.Listen();
            Console.WriteLine("Start to listen the in-memory events");
            Console.WriteLine("Click on a key to quit the application ...");
            Console.ReadLine();
        }
    }
}