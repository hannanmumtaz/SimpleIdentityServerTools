using System;
using System.Collections.Generic;

namespace SimpleIdentityServer.Connectors.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static string TryGetStr(this IDictionary<string, string> dic, string name)
        {
            if (dic == null)
            {
                throw new ArgumentNullException(nameof(dic));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (!dic.ContainsKey(name))
            {
                return null;
            }

            return dic[name];
        }

        public static IEnumerable<string> TryGetStrArr(this IDictionary<string, string> dic, string name)
        {
            var value = dic.TryGetStr(name);
            if (value == null)
            {
                return null;
            }

            return value.Split(';');
        }
    }
}
