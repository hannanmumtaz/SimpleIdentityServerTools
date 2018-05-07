using SimpleIdentityServer.Eid.OpenId.Core.Parameters;
using SimpleIdentityServer.Eid.OpenId.ViewModels;
using System;

namespace SimpleIdentityServer.Eid.OpenId.Extensions
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
