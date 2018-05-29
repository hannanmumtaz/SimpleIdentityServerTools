using SimpleIdentityServer.Rms.Client.DTOs.Responses;
using SimpleIdentityServer.Rms.Client.Licensing.TemplateDistribution;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Rms.Client.Licensing
{
    public interface ILicensingOperations
    {
        Task<AcquireTemplatesResponse> GetAcquireTemplateInformation(string baseUrl);
    }

    internal sealed class LicensingOperations : ILicensingOperations
    {
        private readonly ILicensingAcquireTemplateInformationOperation _licensingAcquireTemplateInformationOperation;

        public LicensingOperations(ILicensingAcquireTemplateInformationOperation licensingAcquireTemplateInformationOperation)
        {
            _licensingAcquireTemplateInformationOperation = licensingAcquireTemplateInformationOperation;
        }

        public Task<AcquireTemplatesResponse> GetAcquireTemplateInformation(string baseUrl)
        {
            return _licensingAcquireTemplateInformationOperation.Execute(baseUrl);
        }
    }
}
