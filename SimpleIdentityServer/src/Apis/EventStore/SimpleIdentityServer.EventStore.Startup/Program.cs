using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SimpleBus.Core;
using SimpleBus.InMemory;
using System.Collections.Generic;

namespace SimpleIdentityServer.EventStore.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();
            /*
            var handlers = new List<IEventHandler>
            {
                new OauthHandler()
            };
            var eventSubscriber = new InMemoryEventSubscriber(new InMemoryOptions(), handlers);
            */
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:60002")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
