using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SimpleIdentityServer.DocumentManagement.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // To launch the application : dotnet run --server.urls=http://*:5000
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:64951")
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
