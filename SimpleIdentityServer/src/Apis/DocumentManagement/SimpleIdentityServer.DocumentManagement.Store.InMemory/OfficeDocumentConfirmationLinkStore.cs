using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleIdentityServer.DocumentManagement.Store.InMemory
{
    internal sealed class OfficeDocumentConfirmationLinkStore : IOfficeDocumentConfirmationLinkStore
    {
        private ICollection<OfficeDocumentConfirmationLink> _confirmationLinks;

        public OfficeDocumentConfirmationLinkStore()
        {
            _confirmationLinks = new List<OfficeDocumentConfirmationLink>();
        }

        public Task<bool> Update(OfficeDocumentConfirmationLink officeDocumentConfirmationLink)
        {
            if (officeDocumentConfirmationLink == null)
            {
                throw new ArgumentNullException(nameof(officeDocumentConfirmationLink));
            }

            var confirmationLink = _confirmationLinks.FirstOrDefault(c => c.ConfirmationCode == officeDocumentConfirmationLink.ConfirmationCode);
            if (confirmationLink != null)
            {
                return Task.FromResult(false);
            }

            confirmationLink.NumberOfConfirmations = officeDocumentConfirmationLink.NumberOfConfirmations;
            confirmationLink.UpdateDateTime = DateTime.UtcNow;
            return Task.FromResult(true);
        }

        public Task<bool> Add(OfficeDocumentConfirmationLink officeDocumentConfirmationLink)
        {
            if (officeDocumentConfirmationLink == null)
            {
                throw new ArgumentNullException(nameof(officeDocumentConfirmationLink));
            }

            var confirmationLink = _confirmationLinks.FirstOrDefault(c => c.ConfirmationCode == officeDocumentConfirmationLink.ConfirmationCode);
            if (confirmationLink != null)
            {
                return Task.FromResult(false);
            }

            officeDocumentConfirmationLink.CreateDateTime = DateTime.UtcNow;
            _confirmationLinks.Add(officeDocumentConfirmationLink);
            return Task.FromResult(true);
        }

        public Task<OfficeDocumentConfirmationLink> Get(string confirmationCode)
        {
            var confirmationLink = _confirmationLinks.FirstOrDefault(c => c.ConfirmationCode == confirmationCode);
            if (confirmationLink == null)
            {
                return Task.FromResult((OfficeDocumentConfirmationLink)null);
            }
            
            return Task.FromResult(confirmationLink);
        }

        public Task<bool> Remove(string confirmationCode)
        {
            var confirmationLink = _confirmationLinks.FirstOrDefault(c => c.ConfirmationCode == confirmationCode);
            if (confirmationLink == null)
            {
                return Task.FromResult(false);
            }

            _confirmationLinks.Remove(confirmationLink);
            return Task.FromResult(true);
        }

        public Task<IEnumerable<OfficeDocumentConfirmationLink>> Search(SearchOfficeDocumentConfirmationLinkParameter parameter)
        {
            if(parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            IEnumerable<OfficeDocumentConfirmationLink> result = _confirmationLinks;
            if(parameter.DocumentIds != null)
            {
                result = result.Where(r => parameter.DocumentIds.Contains(r.DocumentId));
            }

            return Task.FromResult(result);
        }
    }
}
