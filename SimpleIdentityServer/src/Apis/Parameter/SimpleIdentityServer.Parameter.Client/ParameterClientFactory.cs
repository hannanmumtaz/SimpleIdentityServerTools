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
            services.AddTransient<IGetUnitsAction, GetUnitsAction>();
            services.AddTransient<IGetConnectorsAction, GetConnectorsAction>();
            services.AddTransient<IGetTwoFactorsAction, GetTwoFactorsAction>();
            services.AddTransient<IUpdateUnitsAction, UpdateUnitsAction>();
            services.AddTransient<IUpdateConnectorsAction, UpdateConnectorsAction>();
            services.AddTransient<IUpdateTwoFactorsAction, UpdateTwoFactorsAction>();
            return services;
        }
    }
}
