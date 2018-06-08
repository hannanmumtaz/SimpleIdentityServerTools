using Microsoft.Extensions.DependencyInjection;
using SimpleIdentityServer.Common.Client;
using SimpleIdentityServer.DocumentManagement.Client.OfficeDocuments;

namespace SimpleIdentityServer.DocumentManagement.Client
{
    public interface IDocumentManagementFactory
    {
        IOfficeDocumentClient GetOfficeDocumentClient();
    }

    public sealed class DocumentManagementFactory : IDocumentManagementFactory
    {
        private readonly ServiceProvider _servicesProvider;

        public DocumentManagementFactory()
        {
            var services = BuildServiceCollection();
            _servicesProvider =  services.BuildServiceProvider();
        }

        public IOfficeDocumentClient GetOfficeDocumentClient()
        {
            return _servicesProvider.GetService<IOfficeDocumentClient>();
        }

        private IServiceCollection BuildServiceCollection()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommonClient();
            serviceCollection.AddTransient<IOfficeDocumentClient, OfficeDocumentClient>();
            serviceCollection.AddTransient<IUpdateOfficeDocumentOperation, UpdateOfficeDocumentOperation>();
            serviceCollection.AddTransient<IGetOfficeDocumentOperation, GetOfficeDocumentOperation>();
            serviceCollection.AddTransient<IAddOfficeDocumentOperation, AddOfficeDocumentOperation>();
            return serviceCollection;
        }
    }
}
