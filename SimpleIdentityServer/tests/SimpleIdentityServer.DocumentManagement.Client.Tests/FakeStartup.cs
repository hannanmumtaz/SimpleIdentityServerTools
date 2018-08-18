using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.AccessToken.Store;
using SimpleIdentityServer.Client;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.DocumentManagement.Api;
using SimpleIdentityServer.DocumentManagement.Api.Controllers;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.Client.Tests.Extensions;
using SimpleIdentityServer.DocumentManagement.Client.Tests.Middlewares;
using SimpleIdentityServer.DocumentManagement.EF;
using SimpleIdentityServer.DocumentManagement.EF.InMemory;
using SimpleIdentityServer.DocumentManagement.Store;
using SimpleIdentityServer.DocumentManagement.Store.InMemory;
using System;
using System.Reflection;

namespace SimpleIdentityServer.DocumentManagement.Client.Tests
{
    public class FakeStartup : IStartup
    {
        private SharedContext _context;

        public FakeStartup(SharedContext context)
        {
            _context = context;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            var factory = new IdentityServerClientFactory(_context.HttpClientFactory);
            services.AddSingleton<IIdentityServerClientFactory>(factory);
            services.AddSingleton<IAccessTokenStore>(_context.AccessTokenStore.Object);
            services.AddSingleton<IIdentityServerUmaClientFactory>(_context.IdentityServerUmaClientFactory.Object);
            services.AddDocumentManagementEFInMemory();
            services.AddSimpleIdentityServerJwt();
            services.AddDocumentManagementInMemoryStore();
            services.AddDocumentManagementHost(new DocumentManagementApiOptions("wellknown")
            {
                OAuth = new OAuthOptions
                {
                    ClientId = "clientid",
                    ClientSecret = "clientsecret",
                    WellKnownConfiguration = "wellknown"
                }
            });
            services.AddAuthentication("UserInfoIntrospection")
                .AddFakeUserInfoIntrospection(opts => { });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("connected", policy => policy.RequireAssertion((h) => true));
            });
            var mvc = services.AddMvc();
            var parts = mvc.PartManager.ApplicationParts;
            parts.Clear();
            parts.Add(new AssemblyPart(typeof(ConfigurationController).GetTypeInfo().Assembly));
            return services.BuildServiceProvider();
        }

        public void Configure(IApplicationBuilder app)
        {
            var store = app.ApplicationServices.GetService<IOfficeDocumentConfirmationLinkStore>();
            _context.OfficeDocumentConfirmationLinkStore = store;
            app.UseAuthentication();
            app.UseDocumentManagementApi();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var simpleIdentityServerContext = serviceScope.ServiceProvider.GetService<DocumentManagementDbContext>();
                simpleIdentityServerContext.Database.EnsureCreated();
                simpleIdentityServerContext.EnsureSeedData();
            }
        }
    }
}
