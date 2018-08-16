using Microsoft.AspNetCore.Hosting;

namespace SimpleIdentityServer.DocumentManagement.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:60010")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
