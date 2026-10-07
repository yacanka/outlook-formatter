using MailFormatter;
using Ribbon = Microsoft.Office.Tools.Ribbon;

namespace MailFormatter
{
    partial class FormatterRibbonCompose : Ribbon.RibbonBase
    {
        private System.ComponentModel.IContainer components = null;

        // Ribbon kontrolleri
        internal Ribbon.RibbonTab tabMailFormatter;
        internal Ribbon.RibbonGroup grpActions;
        internal Ribbon.RibbonButton btnFormatMail;
        internal Ribbon.RibbonButton btnSettings;
        internal Ribbon.RibbonToggleButton btnShowRaw;

        public FormatterRibbonCompose()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabMailFormatter = this.Factory.CreateRibbonTab();
            this.grpActions = this.Factory.CreateRibbonGroup();
            this.btnFormatMail = this.Factory.CreateRibbonButton();
            this.btnSettings = this.Factory.CreateRibbonButton();
            this.btnShowRaw = this.Factory.CreateRibbonToggleButton();
            this.tabMailFormatter.SuspendLayout();
            this.grpActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabMailFormatter
            // 
            this.tabMailFormatter.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tabMailFormatter.ControlId.OfficeId = "TabNewMailMessage";
            this.tabMailFormatter.Groups.Add(this.grpActions);
            this.tabMailFormatter.Label = "TabNewMailMessage";
            this.tabMailFormatter.Name = "tabMailFormatter";
            // 
            // grpActions
            // 
            this.grpActions.Items.Add(this.btnFormatMail);
            this.grpActions.Items.Add(this.btnSettings);
            this.grpActions.Items.Add(this.btnShowRaw);
            this.grpActions.Label = "Mail İşlemleri";
            this.grpActions.Name = "grpActions";
            // 
            // btnFormatMail
            // 
            this.btnFormatMail.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnFormatMail.Label = "Formatla";
            this.btnFormatMail.Name = "btnFormatMail";
            this.btnFormatMail.OfficeImageId = "CreateMailRule";
            this.btnFormatMail.ScreenTip = "Seçili maili formatla";
            this.btnFormatMail.ShowImage = true;
            this.btnFormatMail.SuperTip = "Seçili mail içeriğini belirlenen formatta yeniden oluşturur.";
            this.btnFormatMail.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnFormatMail_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Label = "Ayarlar";
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.OfficeImageId = "GroupDesign";
            this.btnSettings.ShowImage = true;
            this.btnSettings.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnSettings_Click);
            // 
            // btnShowRaw
            // 
            this.btnShowRaw.Label = "Kodu Göster";
            this.btnShowRaw.Name = "btnShowRaw";
            this.btnShowRaw.OfficeImageId = "DesignAFormOutlook";
            this.btnShowRaw.ShowImage = true;
            this.btnShowRaw.Visible = global::MailFormatter.Properties.Settings.Default.ShowRawRibbonButton;
            this.btnShowRaw.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnShowRaw_Click);
            // 
            // FormatterRibbonCompose
            // 
            this.Name = "FormatterRibbonCompose";
            this.RibbonType = "Microsoft.Outlook.Mail.Compose";
            this.Tabs.Add(this.tabMailFormatter);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.FormatterRibbonCompose_Load);
            this.tabMailFormatter.ResumeLayout(false);
            this.tabMailFormatter.PerformLayout();
            this.grpActions.ResumeLayout(false);
            this.grpActions.PerformLayout();
            this.ResumeLayout(false);

        }
    }

    partial class ThisRibbonCollection
    {
        internal FormatterRibbonCompose FormatterRibbonCompose
        {
            get { return this.GetRibbon<FormatterRibbonCompose>(); }
        }
    }
}