using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("http://*:60006")
                .Build();
            host.Run();
        }
    }
}
