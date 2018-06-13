using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using SimpleIdentityServer.Parameter.Core.Params;
using SimpleIdentityServer.Parameter.Core.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Parameters
{
    public interface IParameterActions
    {
        GetModulesResponse GetModules();
        GetConnectorsResponse GetConnectors();
        bool Update(IEnumerable<UpdateParameter> updateParameters);
        bool Update(IEnumerable<UpdateConnector> updateConnectors);
    }

    internal sealed class ParameterActions : IParameterActions
    {
        private readonly IGetModulesAction _getModulesAction;
        private readonly IGetConnectorsAction _getConnectorsAction;
        private readonly IUpdateModuleConfigurationAction _updateModuleConfigurationAction;
        private readonly IUpdateConnectorsAction _updateConnectorsAction;

        public ParameterActions(IGetModulesAction getModulesAction, IGetConnectorsAction getConnectorsAction, 
            IUpdateModuleConfigurationAction updateModuleConfigurationAction, IUpdateConnectorsAction updateConnectorsAction)
        {
            _getModulesAction = getModulesAction;
            _getConnectorsAction = getConnectorsAction;
            _updateModuleConfigurationAction = updateModuleConfigurationAction;
            _updateConnectorsAction = updateConnectorsAction;
        }

        public GetModulesResponse GetModules()
        {
            return _getModulesAction.Execute();
        }

        public GetConnectorsResponse GetConnectors()
        {
            return _getConnectorsAction.Execute();
        }

        public bool Update(IEnumerable<UpdateParameter> updateParameters)
        {
            return _updateModuleConfigurationAction.Execute(updateParameters);
        }

        public bool Update(IEnumerable<UpdateConnector> updateConnectors)
        {
            return _updateConnectorsAction.Execute(updateConnectors);
        }
    }
}