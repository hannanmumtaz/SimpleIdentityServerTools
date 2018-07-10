using SimpleIdentityServer.Parameter.Core.Parameters.Actions;
using SimpleIdentityServer.Parameter.Core.Params;
using SimpleIdentityServer.Parameter.Core.Responses;
using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Parameters
{
    public interface IParameterActions
    {
        GetModulesResponse GetUnits();
        GetConnectorsResponse GetConnectors();
        GetTwoFactorsResponse GetTwoFactors();
        bool Update(IEnumerable<UpdateParameter> updateParameters);
        bool Update(IEnumerable<UpdateConnector> updateConnectors);
        bool Update(IEnumerable<UpdateTwoFactor> updateTwoFactors);
    }

    internal sealed class ParameterActions : IParameterActions
    {
        private readonly IGetUnitsAction _getUnitsAction;
        private readonly IGetConnectorsAction _getConnectorsAction;
        private readonly IGetTwoFactorsAction _getTwoFactorsAction;
        private readonly IUpdateUnitsAction _updateUnitsAction;
        private readonly IUpdateConnectorsAction _updateConnectorsAction;
        private readonly IUpdateTwoFactorsAction _updateTwoFactorsAction;

        public ParameterActions(IGetUnitsAction getUnitsAction, IGetConnectorsAction getConnectorsAction, IGetTwoFactorsAction getTwoFactorsAction,
            IUpdateUnitsAction updateUnitsAction, IUpdateConnectorsAction updateConnectorsAction, IUpdateTwoFactorsAction updateTwoFactorsAction)
        {
            _getUnitsAction = getUnitsAction;
            _getConnectorsAction = getConnectorsAction;
            _getTwoFactorsAction = getTwoFactorsAction;
            _updateUnitsAction = updateUnitsAction;
            _updateConnectorsAction = updateConnectorsAction;
            _updateTwoFactorsAction = updateTwoFactorsAction;
        }

        public GetModulesResponse GetUnits()
        {
            return _getUnitsAction.Execute();
        }

        public GetConnectorsResponse GetConnectors()
        {
            return _getConnectorsAction.Execute();
        }

        public GetTwoFactorsResponse GetTwoFactors()
        {
            return _getTwoFactorsAction.Execute();
        }

        public bool Update(IEnumerable<UpdateParameter> updateParameters)
        {
            return _updateUnitsAction.Execute(updateParameters);
        }

        public bool Update(IEnumerable<UpdateConnector> updateConnectors)
        {
            return _updateConnectorsAction.Execute(updateConnectors);
        }

        public bool Update(IEnumerable<UpdateTwoFactor> updateTwoFactors)
        {
            return _updateTwoFactorsAction.Execute(updateTwoFactors);
        }
    }
}