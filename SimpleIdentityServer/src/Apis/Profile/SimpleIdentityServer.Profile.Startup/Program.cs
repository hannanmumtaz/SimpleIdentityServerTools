﻿using Microsoft.AspNetCore.Hosting;

namespace SimpleIdentityServer.Profile.Startup
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls("http://*:60005")
                .UseStartup<Startup>()
                .Build();
            host.Run();
        }
    }
}
