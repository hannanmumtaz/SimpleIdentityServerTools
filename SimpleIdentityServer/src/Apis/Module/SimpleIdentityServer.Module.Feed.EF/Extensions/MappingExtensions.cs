using SimpleIdentityServer.Module.Feed.Core.Models;
using SimpleIdentityServer.Module.Feed.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.EF.Extensions
{
    internal static class MappingExtensions
    {
        public static ProjectConnectorAggregate ToDomain(this Connector connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            return new ProjectConnectorAggregate
            {
                Library = connector.Library,
                Name = connector.Name,
                Parameters = string.IsNullOrWhiteSpace(connector.Parameters) ? new string[0] : connector.Parameters.Split(','),
                Version = connector.Version,
                CreateDateTime = connector.CreateDateTime,
                Description = connector.Description,
                Picture = connector.Picture,
                UpdateDateTime = connector.UpdateDateTime
            };
        }

        public static ProjectTwoFactorAuthenticatorAggregate ToDomain(this TwoFactorAuthenticator twoFactorAuthenticator)
        {
            if (twoFactorAuthenticator == null)
            {
                throw new ArgumentNullException(nameof(twoFactorAuthenticator));
            }

            return new ProjectTwoFactorAuthenticatorAggregate
            {
                Id = twoFactorAuthenticator.Description,
                Description = twoFactorAuthenticator.Description,
                Library = twoFactorAuthenticator.Library,
                Name = twoFactorAuthenticator.Name,
                Picture = twoFactorAuthenticator.Picture,
                CreateDateTime = twoFactorAuthenticator.CreateDateTime,
                UpdateDateTime = twoFactorAuthenticator.UpdateDateTime,
                Version = twoFactorAuthenticator.Version,
                Parameters = string.IsNullOrWhiteSpace(twoFactorAuthenticator.Parameters) ? new string[0] : twoFactorAuthenticator.Parameters.Split(',')
            };
        }

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
                Units = project.Units == null ? new List<ProjectUnitAggregate>() : project.Units.Select(u => u.ToDomain()),
                Connectors = project.Connectors == null ? new List<ProjectConnectorAggregate>() : project.Connectors.Select(u => u.ToDomain()),
                TwoFactorAuthenticators = project.TwoFactorAuthenticators == null ? new List<ProjectTwoFactorAuthenticatorAggregate>() : project.TwoFactorAuthenticators.Select(u => u.ToDomain())
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
                Version = unitPackage.Version,
                Parameters = string.IsNullOrWhiteSpace(unitPackage.Parameters) ? new string[0] : unitPackage.Parameters.Split(',')
            };
        }
    }
}
