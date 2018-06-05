namespace WordAccessManagementAddin
{
    partial class Ribbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public Ribbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.tab1 = this.Factory.CreateRibbonTab();
            this.First = this.Factory.CreateRibbonGroup();
            this.login = this.Factory.CreateRibbonButton();
            this.unprotect = this.Factory.CreateRibbonButton();
            this.protect = this.Factory.CreateRibbonButton();
            this.tab1.SuspendLayout();
            this.First.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.First);
            this.tab1.Label = "TabAddIns";
            this.tab1.Name = "tab1";
            // 
            // First
            // 
            this.First.Items.Add(this.login);
            this.First.Items.Add(this.unprotect);
            this.First.Items.Add(this.protect);
            this.First.Label = "First";
            this.First.Name = "First";
            // 
            // login
            // 
            this.login.Label = "login";
            this.login.Name = "login";
            this.login.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnClickLogin);
            // 
            // unprotect
            // 
            this.unprotect.Label = "Unprotected";
            this.unprotect.Name = "unprotect";
            this.unprotect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnUnprotect);
            // 
            // protect
            // 
            this.protect.Label = "Protect";
            this.protect.Name = "protect";
            this.protect.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnProtect);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.OnLoadRibbon);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.First.ResumeLayout(false);
            this.First.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup First;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton protect;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton unprotect;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton login;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
