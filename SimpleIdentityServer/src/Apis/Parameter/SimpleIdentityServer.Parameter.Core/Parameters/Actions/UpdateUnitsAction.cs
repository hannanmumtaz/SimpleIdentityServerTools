using Newtonsoft.Json;
using SimpleIdentityServer.Module.Feed.Common.Responses;
using SimpleIdentityServer.Parameter.Core.Exceptions;
using SimpleIdentityServer.Parameter.Core.Helpers;
using SimpleIdentityServer.Parameter.Core.Params;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IUpdateUnitsAction
    {
        bool Execute(IEnumerable<UpdateParameter> updateParameters);
    }

    internal sealed class UpdateUnitsAction : IUpdateUnitsAction
    {
        private readonly IDirectoryHelper _directoryHelper;

        public UpdateUnitsAction(IDirectoryHelper directoryHelper)
        {
            _directoryHelper = directoryHelper;
        }

        public bool Execute(IEnumerable<UpdateParameter> updateParameters)
        {
            if (updateParameters == null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            var currentDirectory = _directoryHelper.GetCurrentDirectory();
            var configurationPath = Path.Combine(currentDirectory, Constants.ConfigurationFileName);
            var configurationTemplatePath = Path.Combine(currentDirectory, Constants.ConfigurationTemplateFileName);
            var projectConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationPath));
            var projectTemplateConfiguration = JsonConvert.DeserializeObject<ProjectResponse>(File.ReadAllText(configurationTemplatePath));
            if (projectConfiguration == null)
            {
                throw new BadConfigurationException($"the {Constants.ConfigurationFileName} is not correct");
            }

            if (projectTemplateConfiguration == null)
            {
                throw new BadConfigurationException($"the {Constants.ConfigurationTemplateFileName} is not correct");
            }

            if (projectConfiguration.Units == null || !projectConfiguration.Units.Any())
            {
                throw new BadConfigurationException("No units in the configuration file");
            }

            var errors = new List<string>();
            foreach (var updateParameter in updateParameters)
            {
                var projectUnit = projectConfiguration.Units.FirstOrDefault(u => u.UnitName == updateParameter.UnitName);
                if (projectUnit == null)
                {
                    errors.Add($"The unit {updateParameter.UnitName} doesn't exist");
                    continue;
                }

                if (projectUnit.Packages == null)
                {
                    errors.Add($"The unit {updateParameter.UnitName} doesn't contain package");
                    continue;
                }

                var projectUnitPackage = projectUnit.Packages.FirstOrDefault(p => p.CategoryName == updateParameter.CategoryName);
                if (projectUnitPackage == null)
                {
                    errors.Add($"The category {updateParameter.UnitName}\\{updateParameter.CategoryName} doesn't exist");
                    continue;
                }

                var projectTemplateUnit = projectTemplateConfiguration.Units.First(u => u.UnitName == updateParameter.UnitName);
                var projectTemplateUnitPackage = projectTemplateUnit.Packages.FirstOrDefault(p => p.Library == updateParameter.LibraryName && p.CategoryName == updateParameter.CategoryName);
                if (projectTemplateUnitPackage == null)
                {
                    errors.Add($"The library {updateParameter.LibraryName} doesn't exist in the unit {updateParameter.UnitName}");
                    continue;
                }

                projectUnitPackage.Library = updateParameter.LibraryName;
                if (updateParameter.Parameters == null)
                {
                    updateParameter.Parameters = new Dictionary<string, string>();
                    continue;
                }

                foreach (var parameter in updateParameter.Parameters)
                {
                    if (projectUnitPackage.Parameters.ContainsKey(parameter.Key))
                    {
                        projectUnitPackage.Parameters[parameter.Key] = parameter.Value;
                    }
                    else
                    {
                        if (!projectTemplateUnitPackage.Parameters.ContainsKey(parameter.Key))
                        {
                            errors.Add($"The parameter {parameter.Key} doesn't exist in the package {updateParameter.LibraryName}\\{updateParameter.CategoryName}");
                            continue;
                        }

                        projectUnitPackage.Parameters.Add(parameter.Key, parameter.Value);
                    }
                }
            }

            if (errors.Any())
            {
                throw new ParameterAggregateException(errors);
            }

            var json = JsonConvert.SerializeObject(projectConfiguration);
            File.WriteAllText(configurationPath, json);
            return true;
        }
    }
}
