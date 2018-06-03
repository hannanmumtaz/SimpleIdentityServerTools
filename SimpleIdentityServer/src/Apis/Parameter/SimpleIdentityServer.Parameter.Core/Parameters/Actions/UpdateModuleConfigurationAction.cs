using SimpleIdentityServer.Parameter.Core.Params;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleIdentityServer.Parameter.Core.Parameters.Actions
{
    public interface IUpdateModuleConfigurationAction
    {

    }

    internal sealed class UpdateModuleConfigurationAction : IUpdateModuleConfigurationAction
    {
        public async Task<bool> Execute(IEnumerable<UpdateParameter> updateParameters)
        {
            if (updateParameters == null)
            {
                throw new ArgumentNullException(nameof(updateParameters));
            }

            return false;
        }
    }
}
