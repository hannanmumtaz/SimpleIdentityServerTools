using System;
using System.IO;
using System.Windows.Media.Imaging;
using WordAccessManagementAddin.Helpers;

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
            this.manageTab = this.Factory.CreateRibbonTab();
            this.manageGroup = this.Factory.CreateRibbonGroup();
            this.manageAccess = this.Factory.CreateRibbonMenu();
            this.protectButton = this.Factory.CreateRibbonButton();
            this.protectOfflineBtn = this.Factory.CreateRibbonButton();
            this.unprotectOfflineBtn = this.Factory.CreateRibbonButton();
            this.profileButton = this.Factory.CreateRibbonButton();
            this.loginButton = this.Factory.CreateRibbonButton();
            this.disconnectButton = this.Factory.CreateRibbonButton();
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
            this.manageGroup.Items.Add (this.manageAccess);
            this.manageGroup.Label = "accessManagement";
            this.manageGroup.Name = "manageGroup";
            // 
            // manageAccess
            // 
            this.manageAccess.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            var imgPath = Directory.GetCurrentDirectory() + @"\Resources\logo.png";
            this.manageAccess.Image = ResourceHelper.GetImage("WordAccessManagementAddin.Resources.logo.png");
            this.manageAccess.Items.Add(this.protectButton);
            this.manageAccess.Items.Add(this.profileButton);
            this.manageAccess.Items.Add(this.loginButton);
            this.manageAccess.Items.Add(this.disconnectButton);
            this.manageAccess.Items.Add(this.protectOfflineBtn);
            this.manageAccess.Items.Add(this.unprotectOfflineBtn);
            this.manageAccess.Label = "manageAccess";
            this.manageAccess.Name = "manageAccess";
            this.manageAccess.ShowImage = true;
            // 
            // protectButton
            // 
            this.protectButton.Label = "Protect";
            this.protectButton.Name = "protectButton";
            this.protectButton.ShowImage = true;
            this.protectButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleProtect);
            //
            // protectOfflineBtn
            //
            this.protectOfflineBtn.Label = "ProtectOffline";
            this.protectOfflineBtn.Name = "protectOffline";
            this.protectOfflineBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleProtectOffline);
            //
            // protectOfflineBtn
            //
            this.unprotectOfflineBtn.Label = "UnprotectOffline";
            this.unprotectOfflineBtn.Name = "unprotectOffline";
            this.unprotectOfflineBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleUnprotectOffline);
            // 
            // profileButton
            // 
            this.profileButton.Label = "Profile";
            this.profileButton.Name = "profileButton";
            this.profileButton.ShowImage = true;
            this.profileButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleProfile);
            // 
            // loginButton
            // 
            this.loginButton.Label = "Login";
            this.loginButton.Name = "loginButton";
            this.loginButton.ShowImage = true;
            this.loginButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleLogin);
            //
            // disconnectButton
            //
            this.disconnectButton.Label = "Disconnect";
            this.disconnectButton.Name = "disconnectButton";
            this.disconnectButton.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.HandleDisconnect);
            // 
            // Ribbon
            // 
            this.Name = "Ribbon";
            this.RibbonType = "Microsoft.Word.Document";
            this.Tabs.Add(this.manageTab);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.HandleRibbonLoad);
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
        internal Microsoft.Office.Tools.Ribbon.RibbonButton profileButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton loginButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton disconnectButton;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton protectOfflineBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton unprotectOfflineBtn;
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
