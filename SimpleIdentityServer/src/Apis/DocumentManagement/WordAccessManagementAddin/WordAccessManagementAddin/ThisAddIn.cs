using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace WordAccessManagementAddin
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            var s = "";
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        private void DocumentOpen(Word.Document Doc)
        {
        }

        private void Application_DocumentChange()
        {
        }

        private void Application_ProtectedViewWindowOpen(Word.ProtectedViewWindow PvWindow)
        {
        }

        #region Code généré par VSTO

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InternalStartup()
        {
            Startup += new System.EventHandler(ThisAddIn_Startup);
            Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
            Application.DocumentOpen += DocumentOpen;
            Application.DocumentChange += Application_DocumentChange;
            Application.ProtectedViewWindowOpen += Application_ProtectedViewWindowOpen;
        }

        #endregion
    }
}
