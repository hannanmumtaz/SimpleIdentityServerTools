using SimpleIdentityServer.Authenticate.Eid.Core.Parameters;
using SimpleIdentityServer.Authenticate.Eid.ViewModels;
using System;

namespace SimpleIdentityServer.Authenticate.Eid.Extensions
{
    internal static class MappingExtensions
    {
        public static LocalAuthenticateParameter ToParameter(this LoginViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            return new LocalAuthenticateParameter
            {
                Xml = viewModel.Xml
            };
        }

        public static LocalAuthenticateParameter ToParameter(this EidAuthorizeViewModel viewModel)
        {
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(viewModel));
            }

            return new LocalAuthenticateParameter
            {
                Xml = viewModel.Xml
            };
        }
    }
}
