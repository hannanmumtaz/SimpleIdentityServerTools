using SimpleIdentityServer.Module.Feed.Common.Requests;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Feed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    internal static class MappingExtensions
    {
        public static ConnectorAggregate ToParameter(this AddConnectorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return new ConnectorAggregate
            {
                Description = request.Description,
                Library = request.Library,
                Name = request.Name,
                Parameters = request == null ? string.Empty : string.Join(",", request.Parameters),
                Picture = request.Picture,
                Version = request.Version
            };
        }

        public static ConnectorResponse ToDto(this ConnectorAggregate connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            return new ConnectorResponse
            {
                CreateDateTime = connector.CreateDateTime,
                Description = connector.Description,
                Library = connector.Library,
                Name = connector.Name,
                Parameters = string.IsNullOrWhiteSpace(connector.Parameters) ? new string[0] : connector.Parameters.Split(','),
                Picture = connector.Picture,
                UpdateDateTime = connector.UpdateDateTime,
                Version = connector.Version
            };
        }

        public static ProjectResponse ToDto(this ProjectAggregate project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            return new ProjectResponse
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Version = project.Version,
                Units = project.Units == null ? new List<ProjectUnitResponse>() : project.Units.Select(u => u.ToDto())
            };
        }

        public static ProjectUnitResponse ToDto(this ProjectUnitAggregate projectUnitAggregate)
        {
            if (projectUnitAggregate == null)
            {
                throw new ArgumentNullException(nameof(projectUnitAggregate));
            }

            return new ProjectUnitResponse
            {
                UnitName = projectUnitAggregate.UnitName,
                Packages = projectUnitAggregate.Packages == null ? new List<UnitPackageResponse>() : projectUnitAggregate.Packages.Select(p => p.ToDto())
            };
        }

        public static UnitPackageResponse ToDto(this UnitPackageAggregate unitPackageAggregate)
        {
            if (unitPackageAggregate == null)
            {
                throw new ArgumentNullException(nameof(unitPackageAggregate));
            }

            var parameters = new Dictionary<string, string>();
            if (unitPackageAggregate.Parameters != null)
            {
                foreach(var record in unitPackageAggregate.Parameters)
                {
                    parameters.Add(record, string.Empty);
                }
            }

            return new UnitPackageResponse
            {
                CategoryName = unitPackageAggregate.CategoryName,
                Library = unitPackageAggregate.Library,
                Version = unitPackageAggregate.Version,
                Parameters = parameters
            };
        }
    }
}
