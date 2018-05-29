using Microsoft.AspNetCore.Hosting;

namespace SimpleIdentityServer.Module.Feed.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:60008")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
