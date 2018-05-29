using SimpleIdentityServer.Rms.Client.Licensing;

namespace SimpleIdentityServer.Rms.Client
{
    public interface IRmsClient
    {
        ILicensingOperations GetLicensing();
    }

    internal sealed class RmsClient : IRmsClient
    {
        private readonly ILicensingOperations _licensingOperations;

        public RmsClient(ILicensingOperations licensingOperations)
        {
            _licensingOperations = licensingOperations;
        }

        public ILicensingOperations GetLicensing()
        {
            return _licensingOperations;
        }
    }
}
