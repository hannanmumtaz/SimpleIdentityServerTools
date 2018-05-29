using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Feed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    internal static class MappingExtensions
    {
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

            return new UnitPackageResponse
            {
                CategoryName = unitPackageAggregate.CategoryName,
                Library = unitPackageAggregate.Library,
                Version = unitPackageAggregate.Version
            };
        }
    }
}
