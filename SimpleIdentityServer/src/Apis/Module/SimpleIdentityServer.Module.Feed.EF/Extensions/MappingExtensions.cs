﻿using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.EF.Extensions
{
    internal static class MappingExtensions
    {
        public static ProjectAggregate ToDomain(this Project project)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            return new ProjectAggregate
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Version = project.Version,
                Units = project.Units == null ? new List<ProjectUnitAggregate>() : project.Units.Select(u => u.ToDomain())
            };
        }

        public static ProjectUnitAggregate ToDomain(this ProjectUnit projectUnit)
        {
            if (projectUnit == null)
            {
                throw new ArgumentNullException(nameof(projectUnit));
            }

            return new ProjectUnitAggregate
            {
                Packages = projectUnit.Unit == null || projectUnit.Unit.Packages == null ? new List<UnitPackageAggregate>() : projectUnit.Unit.Packages.Select(s => s.ToDomain()),
                UnitName = projectUnit.Unit == null ? null : projectUnit.Unit.UnitName
            };
        }

        public static UnitPackageAggregate ToDomain(this UnitPackage unitPackage)
        {
            if (unitPackage == null)
            {
                throw new ArgumentNullException(nameof(unitPackage));
            }

            return new UnitPackageAggregate
            {
                CategoryName = unitPackage.Category == null ? null : unitPackage.Category.Name,
                Library = unitPackage.Library,
                Version = unitPackage.Version
            };
        }
    }
}