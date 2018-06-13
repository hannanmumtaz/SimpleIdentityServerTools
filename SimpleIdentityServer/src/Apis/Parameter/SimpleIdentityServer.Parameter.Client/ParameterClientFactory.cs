using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;

namespace SimpleIdentityServer.Parameter.Client
{
    public interface IParameterClientFactory
    {
        IParameterClient GetParameterClient();
    }

    public class ParameterClientFactory : IParameterClientFactory
    {
        private readonly ServiceProvider _serviceProvider;

        public ParameterClientFactory()
        {
            var services = GetServiceCollection();
            _serviceProvider = services.BuildServiceProvider();
        }

        public IParameterClient GetParameterClient()
        {
            return _serviceProvider.GetService<IParameterClient>();
        }

        private IServiceCollection GetServiceCollection()
        {
            var services = new ServiceCollection();
            services.AddCommonClient();
            services.AddTransient<IParameterClient, ParameterClient>();
            services.AddTransient<IGetModulesAction, GetModulesAction>();
            services.AddTransient<IGetConnectorsAction, GetConnectorsAction>();
            services.AddTransient<IUpdateModulesAction, UpdateModulesAction>();
            services.AddTransient<IUpdateConnectorsAction, UpdateConnectorsAction>();
            return services;
        }
    }
}
