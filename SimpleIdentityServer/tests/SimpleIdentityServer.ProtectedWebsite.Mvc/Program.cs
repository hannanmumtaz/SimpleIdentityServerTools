using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SimpleIdentityServer.ProtectedWebsite.Mvc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                .Build();
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseConfiguration(configuration)
                .UseStartup<Startup>()
                .UseUrls("http://*:60006")
                .Build();
            host.Run();
        }
    }
}
