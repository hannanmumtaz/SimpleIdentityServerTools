using SimpleIdentityServer.Parameter.Common.DTOs.Requests;
using SimpleIdentityServer.Parameter.Common.DTOs.Results;
using SimpleIdentityServer.Parameter.Core.Params;
using SimpleIdentityServer.Parameter.Core.Responses;
using System;

namespace SimpleIdentityServer.Parameter.Host.Extensions
{
    internal static class MappingExtensions
    {
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

        public static GetModulesResult ToDto(this GetModulesResponse resp)
        {
            if (resp == null)
            {
                throw new ArgumentNullException(nameof(resp));
            }

            return new GetModulesResult
            {
                TemplateUnits = resp.ProjectTemplateUnits,
                Units = resp.ProjectUnits
            };
        }
    }
}
