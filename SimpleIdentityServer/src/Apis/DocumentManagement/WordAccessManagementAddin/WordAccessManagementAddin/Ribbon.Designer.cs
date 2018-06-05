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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Ribbon));
            this.manageTab = this.Factory.CreateRibbonTab();
            this.manageGroup = this.Factory.CreateRibbonGroup();
            this.manageAccess = this.Factory.CreateRibbonMenu();
            this.protectButton = this.Factory.CreateRibbonButton();
            this.unprotectButton = this.Factory.CreateRibbonButton();
            this.loginButton = this.Factory.CreateRibbonButton();
            this.manageTab.SuspendLayout();
            this.manageGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // manageTab
            // 
            this.manageTab.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.manageTab.Groups.Add(this.manageGroup);
            this.manageTab.Label = "TabAddIns";
            this.manageTab.Name = "manageTab";
            // 
            // manageGroup
            // 
            this.manageGroup.Items.Add(this.manageAccess);
            this.manageGroup.Label = "accessManagement";
            this.manageGroup.Name = "manageGroup";
            // 
            // manageAccess
            // 
            this.manageAccess.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.manageAccess.Image = ((System.Drawing.Image)(resources.GetObject("manageAccess.Image")));
            this.manageAccess.Items.Add(this.protectButton);
            this.manageAccess.Items.Add(this.unprotectButton);
            this.manageAccess.Items.Add(this.loginButton);
            this.manageAccess.Label = "manageAccess";
            this.manageAccess.Name = "manageAccess";
            this.manageAccess.ShowImage = true;
            // 
            // protectButton
            // 
            this.protectButton.Label = "Protect";
            this.protectButton.Name = "protectButton";
            this.protectButton.ShowImage = true;
            this.protectButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnProtect);
            // 
            // unprotectButton
            // 
            this.unprotectButton.Label = "Unprotect";
            this.unprotectButton.Name = "unprotectButton";
            this.unprotectButton.ShowImage = true;
            this.unprotectButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnUnprotect);
            // 
            // loginButton
            // 
            this.loginButton.Label = "Login";
            this.loginButton.Name = "loginButton";
            this.loginButton.ShowImage = true;
            this.loginButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OnClickLogin);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.manageTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.OnLoadRibbon);
            this.manageTab.ResumeLayout(false);
            this.manageTab.PerformLayout();
            this.manageGroup.ResumeLayout(false);
            this.manageGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab manageTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup manageGroup;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton protectButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton unprotectButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton loginButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonMenu manageAccess;
    }

    partial class ThisRibbonCollection
    {
        internal Ribbon Ribbon
        {
            get { return this.GetRibbon<Ribbon>(); }
        }
    }
}
