using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleIdentityServer.AccessToken.Store.InMemory;
using SimpleIdentityServer.Core.Jwt;
using SimpleIdentityServer.DocumentManagement.Api;
using SimpleIdentityServer.DocumentManagement.Api.Extensions;
using SimpleIdentityServer.DocumentManagement.EF;
using SimpleIdentityServer.DocumentManagement.EF.InMemory;
using SimpleIdentityServer.DocumentManagement.Startup.Extensions;
using SimpleIdentityServer.DocumentManagement.Store.InMemory;
using SimpleIdentityServer.Uma.Client;
using SimpleIdentityServer.UserInfoIntrospection;

namespace SimpleIdentityServer.DocumentManagement.Startup
{

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddInMemoryAccessTokenStore();
            services.AddUmaClient();
            services.AddSimpleIdentityServerJwt();
            services.AddDocumentManagementEFInMemory();
            services.AddDocumentManagementInMemoryStore();
            services.AddDocumentManagementHost(new DocumentManagementApiOptions("http://localhost:60000/.well-known/openid-configuration")
            {
                OAuth = new OAuthOptions
                {
                    ClientId = "DocumentManagementApi",
                    ClientSecret = "QZhq68aE44BmYEX9",
                    WellKnownConfiguration = "http://localhost:60004/.well-known/uma2-configuration"
                },
                DocumentManagementWebsiteBaseUrl = "http://localhost:64951"
            });

            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()));
            services.AddAuthentication(UserInfoIntrospectionOptions.AuthenticationScheme)
                .AddUserInfoIntrospection(opts =>
                {
                    opts.WellKnownConfigurationUrl = "http://localhost:60000/.well-known/openid-configuration";
                });
            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("connected", policy => policy.RequireAuthenticatedUser());
            });
            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseAuthentication();
            app.UseCors("AllowAll");
            loggerFactory.AddConsole();
            app.UseStatusCodePages();
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
