using Microsoft.Office.Interop.Word;
using System;

namespace WordAccessManagementAddin.Extensions
{
    internal static class DocumentExtension
    {
        public static bool TryGetVariable(this Document document, string name, out string value)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            foreach(Variable variable in document.Variables)
            {
                if (variable.Name == name)
                {
                    value = variable.Value;
                    return true;
                }
            }

            value = null;
            return false;
        }
    }
}