using System.Collections.Generic;

namespace SimpleIdentityServer.Parameter.Core.Exceptions
{
    public class ParameterAggregateException : BaseParameterException
    {
        public ParameterAggregateException(IEnumerable<string> messages)
        {
            Messages = messages;
        }

        public IEnumerable<string> Messages { get; private set; }
    }
}
