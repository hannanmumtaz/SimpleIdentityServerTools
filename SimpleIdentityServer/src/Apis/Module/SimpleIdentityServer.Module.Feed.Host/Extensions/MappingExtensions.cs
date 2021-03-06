﻿using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Module.Feed.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleIdentityServer.Module.Feed.Host.Extensions
{
    internal static class MappingExtensions
    {
        public static ProjectTwoFactorAuthenticator ToDto(this ProjectTwoFactorAuthenticatorAggregate twoFactor)
        {
            if (twoFactor == null)
            {
                throw new ArgumentNullException(nameof(twoFactor));
            }

            var parameters = new Dictionary<string, string>();
            if (twoFactor.Parameters != null)
            {
                foreach (var record in twoFactor.Parameters)
                {
                    parameters.Add(record, string.Empty);
                }
            }
            return new ProjectTwoFactorAuthenticator
            {
                CreateDateTime = twoFactor.CreateDateTime,
                Description = twoFactor.Description,
                Library = twoFactor.Library,
                Name = twoFactor.Name,
                Parameters = parameters,
                Picture = twoFactor.Picture,
                UpdateDateTime = twoFactor.UpdateDateTime,
                Version = twoFactor.Version
            };
        }

        public static ProjectConnectorResponse ToDto(this ProjectConnectorAggregate connector)
        {
            if (connector == null)
            {
                throw new ArgumentNullException(nameof(connector));
            }

            var parameters = new Dictionary<string, string>();
            if (connector.Parameters != null)
            {
                foreach (var record in connector.Parameters)
                {
                    parameters.Add(record, string.Empty);
                }
            }
            return new ProjectConnectorResponse
            {
                CreateDateTime = connector.CreateDateTime,
                Description = connector.Description,
                Library = connector.Library,
                Name = connector.Name,
                Parameters = parameters,
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
                Units = project.Units == null ? new List<ProjectUnitResponse>() : project.Units.Select(u => u.ToDto()),
                Connectors = project.Connectors == null ? new List<ProjectConnectorResponse>() : project.Connectors.Select(u => u.ToDto()),
                TwoFactors = project.TwoFactorAuthenticators == null ? new List<ProjectTwoFactorAuthenticator>() : project.TwoFactorAuthenticators.Select(u => u.ToDto())
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
