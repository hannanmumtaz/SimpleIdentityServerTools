using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Store
{
    public interface IOfficeDocumentConfirmationLinkStore
    {
        Task<bool> Add(OfficeDocumentConfirmationLink officeDocumentConfirmationLink);
        Task<OfficeDocumentConfirmationLink> Get(string confirmationCode);
    }
}
