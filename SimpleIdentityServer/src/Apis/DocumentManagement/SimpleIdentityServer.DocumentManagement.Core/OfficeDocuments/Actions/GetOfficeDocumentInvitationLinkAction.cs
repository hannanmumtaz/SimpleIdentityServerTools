using SimpleIdentityServer.DocumentManagement.Core.Exceptions;
using SimpleIdentityServer.DocumentManagement.Store;
using System;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Core.OfficeDocuments.Actions
{
    public interface IGetOfficeDocumentInvitationLinkAction
    {
        Task<OfficeDocumentConfirmationLink> Execute(string code);
    }

    internal sealed class GetOfficeDocumentInvitationLinkAction : IGetOfficeDocumentInvitationLinkAction
    {
        private readonly IOfficeDocumentConfirmationLinkStore _officeDocumentConfirmationLinkStore;

        public GetOfficeDocumentInvitationLinkAction(IOfficeDocumentConfirmationLinkStore officeDocumentConfirmationLinkStore)
        {
            _officeDocumentConfirmationLinkStore = officeDocumentConfirmationLinkStore;
        }

        public async Task<OfficeDocumentConfirmationLink> Execute(string code)
        {
            if(string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var confirmationLink = await _officeDocumentConfirmationLinkStore.Get(code);
            if(confirmationLink == null)
            {
                throw new ConfirmationCodeNotFoundException();
            }

            return confirmationLink;
        }
    }
}
