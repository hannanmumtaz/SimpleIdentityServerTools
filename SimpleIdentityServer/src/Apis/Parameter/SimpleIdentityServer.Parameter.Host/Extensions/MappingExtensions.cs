using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using SimpleIdentityServer.Parameter.Core.Params;
using SimpleIdentityServer.Parameter.Core.Responses;
using System;

namespace SimpleIdentityServer.Parameter.Host.Extensions
{
    internal static class MappingExtensions
    {
        public static UpdateTwoFactor ToParameter(this UpdateTwoFactorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new UpdateTwoFactor
            {
                Library = request.Library,
                Name = request.Name,
                Parameters = request.Parameters,
                Version = request.Version
            };
        }

        public static UpdateConnector ToParameter(this UpdateConnectorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new UpdateConnector
            {
                Library = request.Library,
                Name = request.Name,
                Parameters = request.Parameters,
                Version = request.Version
            };
        }

        public static UpdateParameter ToParameter(this UpdateParameterRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new UpdateParameter
            {
                CategoryName = request.CategoryName,
                LibraryName = request.LibraryName,
                Parameters = request.Parameters,
                UnitName = request.UnitName
            };
        }

        public static GetUnitsResult ToDto(this GetModulesResponse resp)
        {
            if (resp == null)
            {
                throw new ArgumentNullException(nameof(resp));
            }

            return new GetUnitsResult
            {
                TemplateUnits = resp.ProjectTemplateUnits,
                Units = resp.ProjectUnits
            };
        }

        public static GetConnectorsResult ToDto(this GetConnectorsResponse resp)
        {
            if (resp == null)
            {
                throw new ArgumentNullException(nameof(resp));
            }

            return new GetConnectorsResult
            {
                Connectors = resp.Connectors,
                TemplateConnectors = resp.TemplateConnectors
            };
        }

        public static GetTwoFactorsResult ToDto(this GetTwoFactorsResponse resp)
        {
            if (resp == null)
            {
                throw new ArgumentNullException(nameof(resp));
            }

            return new GetTwoFactorsResult
            {
                TwoFactors = resp.ProjectTwoFactors,
                TemplateTwoFactors = resp.ProjectTemplateTwoFactors
            };
        }
    }
}
