using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using SimpleIdentityServer.Parameter.Core.Params;
using SimpleIdentityServer.Parameter.Core.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Parameters
{
    public interface IParameterActions
    {
        GetModulesResponse GetModules();
        bool Update(IEnumerable<UpdateParameter> updateParameters);
    }

    internal sealed class ParameterActions : IParameterActions
    {
        private readonly IGetModulesAction _getModulesAction;
        private readonly IUpdateModuleConfigurationAction _updateModuleConfigurationAction;

        public ParameterActions(IGetModulesAction getModulesAction, IUpdateModuleConfigurationAction updateModuleConfigurationAction)
        {
            _getModulesAction = getModulesAction;
            _updateModuleConfigurationAction = updateModuleConfigurationAction;
        }

        public GetModulesResponse GetModules()
        {
            return _getModulesAction.Execute();
        }

        public bool Update(IEnumerable<UpdateParameter> updateParameters)
        {
            return _updateModuleConfigurationAction.Execute(updateParameters);
        }
    }
}
