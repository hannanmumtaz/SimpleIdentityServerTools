using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.Common.Saml.Serializers;
using SimpleIdentityServer.Rms.Client.Licensing;
using SimpleIdentityServer.Rms.Client.Licensing.TemplateDistribution;

namespace SimpleIdentityServer.Rms.Client
{
    public class RmsClientFactory
    {
        private ServiceProvider _services;

        public RmsClientFactory()
        {
            var services = BuildServiceCollection();
            _services = services.BuildServiceProvider();
        }

        public IRmsClient GetRmsClient()
        {
            return _services.GetService<IRmsClient>();
        }

        private IServiceCollection BuildServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddCommonClient();
            services.AddTransient<IRmsClient, RmsClient>();
            services.AddTransient<ISoapMessageSerializer, SoapMessageSerializer>();
            services.AddTransient<ILicensingOperations, LicensingOperations>();
            services.AddTransient<ILicensingAcquireTemplateInformationOperation, LicensingAcquireTemplateInformationOperation>();
            return services;
        }
    }
}
