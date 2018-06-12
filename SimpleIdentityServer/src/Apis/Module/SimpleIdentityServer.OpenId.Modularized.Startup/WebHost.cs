using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SimpleIdentityServer.OpenId.Modularized.Startup
{
    internal static class WebHost
    {
        private static IWebHost _webHost;

        public static void Start()
        {
            _webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls("http://*:60000")
                .UseStartup<Startup>()
                .Build();
            _webHost.Run();
        }

        public static void Stop()
        {
            _webHost.StopAsync().Wait();
        }

        public static void Restart()
        {
            Stop();
            Start();
        }
    }
}
