﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace SimpleIdentityServer.Parameters.Host.Tests
{
    public class ParameterServerFixture : IDisposable
    {
        public TestServer Server { get; }
        public HttpClient Client { get; }

        public ParameterServerFixture()
        {
            var startup = new FakeStartup();
            Server = new TestServer(new WebHostBuilder()
                .UseUrls("http://localhost:5000")
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IStartup>(startup);
                })
                .UseSetting(WebHostDefaults.ApplicationKey, typeof(FakeStartup).GetType().Assembly.FullName));
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server.Dispose();
            Client.Dispose();
        }
    }
}
