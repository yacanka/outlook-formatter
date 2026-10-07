using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MailFormatter.Forms
{
    partial class SettingsForm
    {
        private System.ComponentModel.IContainer components = null;

        private readonly string[] baseTemplateItems = new string[] {
            "Uçuş Elverişlilik Bildirisi",
        };

        private readonly string[] otherTemplateItems = new string[] {
            "Akademi Direktörlüğü",
            "Atama Duyurusu",
            "Dış Ticaret ve Destek Hizmetleri",
            "İnsan Kaynakları ve Yetenek Yönetimi Direktörlüğü",
            "Kurumsal Güvenlik ve Acil Durum Direktörlüğü",
            "Kurumsal Çözümler Direktörlüğü",
            "Mühendislik Çözümleri - Kırmızı Ağ Hizmet Kesintisi",
            "Mühendislik Çözümleri - Siyah Ağ Hizmet Kesintisi",
            "Tedarik ve Sanayileşme Başkanlığı",
        };

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.grpAlign = new System.Windows.Forms.GroupBox();
            this.verticalPanel = new System.Windows.Forms.Panel();
            this.radioVerticalTop = new System.Windows.Forms.RadioButton();
            this.radioVerticalMid = new System.Windows.Forms.RadioButton();
            this.radioVerticalBottom = new System.Windows.Forms.RadioButton();
            this.horizontalPanel = new System.Windows.Forms.Panel();
            this.radioHorizontalLeft = new System.Windows.Forms.RadioButton();
            this.radioHorizontalRight = new System.Windows.Forms.RadioButton();
            this.radioHorizontalMid = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.grpCompany = new System.Windows.Forms.GroupBox();
            this.chkShowAllTemplate = new System.Windows.Forms.CheckBox();
            this.templateBox = new System.Windows.Forms.ComboBox();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.tabAppearance = new System.Windows.Forms.TabPage();
            this.chkIncludeFooter = new System.Windows.Forms.CheckBox();
            this.grpFooterText = new System.Windows.Forms.GroupBox();
            this.txtFooterText = new System.Windows.Forms.TextBox();
            this.lblFooterText = new System.Windows.Forms.Label();
            this.grpColors = new System.Windows.Forms.GroupBox();
            this.lblHeaderColor = new System.Windows.Forms.Label();
            this.txtHeaderColor = new System.Windows.Forms.TextBox();
            this.pnlColorPreview = new System.Windows.Forms.Panel();
            this.grpFont = new System.Windows.Forms.GroupBox();
            this.lblFontFamily = new System.Windows.Forms.Label();
            this.cmbFontFamily = new System.Windows.Forms.ComboBox();
            this.lblFontSize = new System.Windows.Forms.Label();
            this.numFontSize = new System.Windows.Forms.NumericUpDown();
            this.tabAdvanced = new System.Windows.Forms.TabPage();
            this.grpLogo = new System.Windows.Forms.GroupBox();
            this.chkIncludeLogo = new System.Windows.Forms.CheckBox();
            this.lblLogoPath = new System.Windows.Forms.Label();
            this.txtLogoPath = new System.Windows.Forms.TextBox();
            this.btnBrowseLogo = new System.Windows.Forms.Button();
            this.picLogoPreview = new System.Windows.Forms.PictureBox();
            this.chkShowRawRibbon = new System.Windows.Forms.CheckBox();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.pnlHeader.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.grpAlign.SuspendLayout();
            this.verticalPanel.SuspendLayout();
            this.horizontalPanel.SuspendLayout();
            this.grpCompany.SuspendLayout();
            this.tabAppearance.SuspendLayout();
            this.grpFooterText.SuspendLayout();
            this.grpColors.SuspendLayout();
            this.grpFont.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).BeginInit();
            this.tabAdvanced.SuspendLayout();
            this.grpLogo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(200, 100);
            this.pnlMain.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(415, 52);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Image = global::MailFormatter.Properties.Resources.spaceBanner;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(415, 52);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mailify Ayarları";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnSave);
            this.pnlFooter.Controls.Add(this.btnCancel);
            this.pnlFooter.Controls.Add(this.btnReset);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 357);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(415, 43);
            this.pnlFooter.TabIndex = 2;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(85)))), ((int)(((byte)(163)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(2)))), ((int)(((byte)(100)))), ((int)(((byte)(191)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(231, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 26);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Kaydet";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(321, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(86, 26);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "İptal";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnReset
            // 
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Location = new System.Drawing.Point(10, 10);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(86, 26);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Sıfırla";
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabGeneral);
            this.tabControl.Controls.Add(this.tabAppearance);
            this.tabControl.Controls.Add(this.tabAdvanced);
            this.tabControl.Location = new System.Drawing.Point(10, 62);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(394, 277);
            this.tabControl.TabIndex = 1;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.grpAlign);
            this.tabGeneral.Controls.Add(this.grpCompany);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(9);
            this.tabGeneral.Size = new System.Drawing.Size(386, 251);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "Genel";
            // 
            // grpAlign
            // 
            this.grpAlign.Controls.Add(this.verticalPanel);
            this.grpAlign.Controls.Add(this.horizontalPanel);
            this.grpAlign.Controls.Add(this.label2);
            this.grpAlign.Controls.Add(this.label1);
            this.grpAlign.Location = new System.Drawing.Point(12, 103);
            this.grpAlign.Name = "grpAlign";
            this.grpAlign.Size = new System.Drawing.Size(362, 78);
            this.grpAlign.TabIndex = 6;
            this.grpAlign.TabStop = false;
            this.grpAlign.Text = "Hizalama";
            // 
            // verticalPanel
            // 
            this.verticalPanel.Controls.Add(this.radioVerticalTop);
            this.verticalPanel.Controls.Add(this.radioVerticalMid);
            this.verticalPanel.Controls.Add(this.radioVerticalBottom);
            this.verticalPanel.Location = new System.Drawing.Point(69, 50);
            this.verticalPanel.Name = "verticalPanel";
            this.verticalPanel.Size = new System.Drawing.Size(200, 22);
            this.verticalPanel.TabIndex = 0;
            // 
            // radioVerticalTop
            // 
            this.radioVerticalTop.AutoSize = true;
            this.radioVerticalTop.Location = new System.Drawing.Point(3, 3);
            this.radioVerticalTop.Name = "radioVerticalTop";
            this.radioVerticalTop.Size = new System.Drawing.Size(41, 17);
            this.radioVerticalTop.TabIndex = 11;
            this.radioVerticalTop.TabStop = true;
            this.radioVerticalTop.Tag = "top";
            this.radioVerticalTop.Text = "Üst";
            this.radioVerticalTop.UseVisualStyleBackColor = true;
            // 
            // radioVerticalMid
            // 
            this.radioVerticalMid.AutoSize = true;
            this.radioVerticalMid.Location = new System.Drawing.Point(50, 3);
            this.radioVerticalMid.Name = "radioVerticalMid";
            this.radioVerticalMid.Size = new System.Drawing.Size(45, 17);
            this.radioVerticalMid.TabIndex = 12;
            this.radioVerticalMid.TabStop = true;
            this.radioVerticalMid.Tag = "middle";
            this.radioVerticalMid.Text = "Orta";
            this.radioVerticalMid.UseVisualStyleBackColor = true;
            // 
            // radioVerticalBottom
            // 
            this.radioVerticalBottom.AutoSize = true;
            this.radioVerticalBottom.Location = new System.Drawing.Point(101, 3);
            this.radioVerticalBottom.Name = "radioVerticalBottom";
            this.radioVerticalBottom.Size = new System.Drawing.Size(37, 17);
            this.radioVerticalBottom.TabIndex = 13;
            this.radioVerticalBottom.TabStop = true;
            this.radioVerticalBottom.Tag = "bottom";
            this.radioVerticalBottom.Text = "Alt";
            this.radioVerticalBottom.UseVisualStyleBackColor = true;
            // 
            // horizontalPanel
            // 
            this.horizontalPanel.Controls.Add(this.radioHorizontalLeft);
            this.horizontalPanel.Controls.Add(this.radioHorizontalRight);
            this.horizontalPanel.Controls.Add(this.radioHorizontalMid);
            this.horizontalPanel.Location = new System.Drawing.Point(69, 22);
            this.horizontalPanel.Name = "horizontalPanel";
            this.horizontalPanel.Size = new System.Drawing.Size(200, 22);
            this.horizontalPanel.TabIndex = 7;
            // 
            // radioHorizontalLeft
            // 
            this.radioHorizontalLeft.AutoSize = true;
            this.radioHorizontalLeft.Location = new System.Drawing.Point(3, 3);
            this.radioHorizontalLeft.Name = "radioHorizontalLeft";
            this.radioHorizontalLeft.Size = new System.Drawing.Size(40, 17);
            this.radioHorizontalLeft.TabIndex = 7;
            this.radioHorizontalLeft.TabStop = true;
            this.radioHorizontalLeft.Tag = "left";
            this.radioHorizontalLeft.Text = "Sol";
            this.radioHorizontalLeft.UseVisualStyleBackColor = true;
            // 
            // radioHorizontalRight
            // 
            this.radioHorizontalRight.AutoSize = true;
            this.radioHorizontalRight.Location = new System.Drawing.Point(100, 3);
            this.radioHorizontalRight.Name = "radioHorizontalRight";
            this.radioHorizontalRight.Size = new System.Drawing.Size(44, 17);
            this.radioHorizontalRight.TabIndex = 9;
            this.radioHorizontalRight.TabStop = true;
            this.radioHorizontalRight.Tag = "right";
            this.radioHorizontalRight.Text = "Sağ";
            this.radioHorizontalRight.UseVisualStyleBackColor = true;
            // 
            // radioHorizontalMid
            // 
            this.radioHorizontalMid.AutoSize = true;
            this.radioHorizontalMid.Location = new System.Drawing.Point(49, 3);
            this.radioHorizontalMid.Name = "radioHorizontalMid";
            this.radioHorizontalMid.Size = new System.Drawing.Size(45, 17);
            this.radioHorizontalMid.TabIndex = 8;
            this.radioHorizontalMid.TabStop = true;
            this.radioHorizontalMid.Tag = "middle";
            this.radioHorizontalMid.Text = "Orta";
            this.radioHorizontalMid.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Dikey:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Yatay:";
            // 
            // grpCompany
            // 
            this.grpCompany.Controls.Add(this.chkShowAllTemplate);
            this.grpCompany.Controls.Add(this.templateBox);
            this.grpCompany.Controls.Add(this.lblCompanyName);
            this.grpCompany.Location = new System.Drawing.Point(9, 9);
            this.grpCompany.Name = "grpCompany";
            this.grpCompany.Size = new System.Drawing.Size(365, 78);
            this.grpCompany.TabIndex = 0;
            this.grpCompany.TabStop = false;
            this.grpCompany.Text = "Format Bilgileri";
            // 
            // chkShowAllTemplate
            // 
            this.chkShowAllTemplate.AutoSize = true;
            this.chkShowAllTemplate.Location = new System.Drawing.Point(13, 22);
            this.chkShowAllTemplate.Name = "chkShowAllTemplate";
            this.chkShowAllTemplate.Size = new System.Drawing.Size(130, 17);
            this.chkShowAllTemplate.TabIndex = 5;
            this.chkShowAllTemplate.Text = "Tüm Şablonları Göster";
            this.chkShowAllTemplate.CheckedChanged += new System.EventHandler(this.chkShowAllTemplate_CheckedChanged);
            // 
            // templateBox
            // 
            this.templateBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.templateBox.FormattingEnabled = true;
            this.templateBox.Items.AddRange(new object[] {
            "Uçuş Elverişlilik Bildirisi"});
            this.templateBox.Location = new System.Drawing.Point(72, 45);
            this.templateBox.Name = "templateBox";
            this.templateBox.Size = new System.Drawing.Size(272, 21);
            this.templateBox.TabIndex = 4;
            this.templateBox.SelectedIndexChanged += new System.EventHandler(this.templateBox_SelectedIndexChanged);
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Location = new System.Drawing.Point(13, 48);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(53, 13);
            this.lblCompanyName.TabIndex = 0;
            this.lblCompanyName.Text = "Formatlar:";
            // 
            // tabAppearance
            // 
            this.tabAppearance.Controls.Add(this.chkIncludeFooter);
            this.tabAppearance.Controls.Add(this.grpFooterText);
            this.tabAppearance.Controls.Add(this.grpColors);
            this.tabAppearance.Controls.Add(this.grpFont);
            this.tabAppearance.Location = new System.Drawing.Point(4, 22);
            this.tabAppearance.Name = "tabAppearance";
            this.tabAppearance.Padding = new System.Windows.Forms.Padding(9);
            this.tabAppearance.Size = new System.Drawing.Size(386, 251);
            this.tabAppearance.TabIndex = 1;
            this.tabAppearance.Text = "Footer";
            // 
            // chkIncludeFooter
            // 
            this.chkIncludeFooter.AutoSize = true;
            this.chkIncludeFooter.Location = new System.Drawing.Point(12, 12);
            this.chkIncludeFooter.Name = "chkIncludeFooter";
            this.chkIncludeFooter.Size = new System.Drawing.Size(79, 17);
            this.chkIncludeFooter.TabIndex = 5;
            this.chkIncludeFooter.Text = "Footer ekle";
            this.chkIncludeFooter.CheckedChanged += new System.EventHandler(this.chkIncludeFooter_CheckedChanged);
            // 
            // grpFooterText
            // 
            this.grpFooterText.Controls.Add(this.txtFooterText);
            this.grpFooterText.Controls.Add(this.lblFooterText);
            this.grpFooterText.Location = new System.Drawing.Point(12, 42);
            this.grpFooterText.Name = "grpFooterText";
            this.grpFooterText.Size = new System.Drawing.Size(360, 60);
            this.grpFooterText.TabIndex = 4;
            this.grpFooterText.TabStop = false;
            this.grpFooterText.Text = "Metin Ayarları";
            // 
            // txtFooterText
            // 
            this.txtFooterText.Location = new System.Drawing.Point(103, 28);
            this.txtFooterText.Name = "txtFooterText";
            this.txtFooterText.Size = new System.Drawing.Size(241, 20);
            this.txtFooterText.TabIndex = 5;
            // 
            // lblFooterText
            // 
            this.lblFooterText.AutoSize = true;
            this.lblFooterText.Location = new System.Drawing.Point(13, 31);
            this.lblFooterText.Name = "lblFooterText";
            this.lblFooterText.Size = new System.Drawing.Size(69, 13);
            this.lblFooterText.TabIndex = 4;
            this.lblFooterText.Text = "Footer Metni:";
            // 
            // grpColors
            // 
            this.grpColors.Controls.Add(this.lblHeaderColor);
            this.grpColors.Controls.Add(this.txtHeaderColor);
            this.grpColors.Controls.Add(this.pnlColorPreview);
            this.grpColors.Location = new System.Drawing.Point(12, 108);
            this.grpColors.Name = "grpColors";
            this.grpColors.Size = new System.Drawing.Size(360, 60);
            this.grpColors.TabIndex = 0;
            this.grpColors.TabStop = false;
            this.grpColors.Text = "Renk Ayarları";
            // 
            // lblHeaderColor
            // 
            this.lblHeaderColor.AutoSize = true;
            this.lblHeaderColor.Location = new System.Drawing.Point(13, 26);
            this.lblHeaderColor.Name = "lblHeaderColor";
            this.lblHeaderColor.Size = new System.Drawing.Size(71, 13);
            this.lblHeaderColor.TabIndex = 0;
            this.lblHeaderColor.Text = "Footer Rengi:";
            // 
            // txtHeaderColor
            // 
            this.txtHeaderColor.Location = new System.Drawing.Point(103, 23);
            this.txtHeaderColor.Name = "txtHeaderColor";
            this.txtHeaderColor.Size = new System.Drawing.Size(75, 20);
            this.txtHeaderColor.TabIndex = 1;
            this.txtHeaderColor.TextChanged += new System.EventHandler(this.txtHeaderColor_TextChanged);
            // 
            // pnlColorPreview
            // 
            this.pnlColorPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlColorPreview.Location = new System.Drawing.Point(194, 23);
            this.pnlColorPreview.Name = "pnlColorPreview";
            this.pnlColorPreview.Size = new System.Drawing.Size(148, 20);
            this.pnlColorPreview.TabIndex = 3;
            this.pnlColorPreview.Click += new System.EventHandler(this.btnPickColor_Click);
            // 
            // grpFont
            // 
            this.grpFont.Controls.Add(this.lblFontFamily);
            this.grpFont.Controls.Add(this.cmbFontFamily);
            this.grpFont.Controls.Add(this.lblFontSize);
            this.grpFont.Controls.Add(this.numFontSize);
            this.grpFont.Location = new System.Drawing.Point(14, 174);
            this.grpFont.Name = "grpFont";
            this.grpFont.Size = new System.Drawing.Size(360, 60);
            this.grpFont.TabIndex = 1;
            this.grpFont.TabStop = false;
            this.grpFont.Text = "Yazı Tipi Ayarları";
            // 
            // lblFontFamily
            // 
            this.lblFontFamily.AutoSize = true;
            this.lblFontFamily.Location = new System.Drawing.Point(13, 26);
            this.lblFontFamily.Name = "lblFontFamily";
            this.lblFontFamily.Size = new System.Drawing.Size(50, 13);
            this.lblFontFamily.TabIndex = 0;
            this.lblFontFamily.Text = "Yazı Tipi:";
            // 
            // cmbFontFamily
            // 
            this.cmbFontFamily.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFontFamily.Location = new System.Drawing.Point(103, 23);
            this.cmbFontFamily.Name = "cmbFontFamily";
            this.cmbFontFamily.Size = new System.Drawing.Size(129, 21);
            this.cmbFontFamily.TabIndex = 1;
            // 
            // lblFontSize
            // 
            this.lblFontSize.AutoSize = true;
            this.lblFontSize.Location = new System.Drawing.Point(249, 26);
            this.lblFontSize.Name = "lblFontSize";
            this.lblFontSize.Size = new System.Drawing.Size(37, 13);
            this.lblFontSize.TabIndex = 2;
            this.lblFontSize.Text = "Boyut:";
            // 
            // numFontSize
            // 
            this.numFontSize.Location = new System.Drawing.Point(291, 23);
            this.numFontSize.Maximum = new decimal(new int[] {
            36,
            0,
            0,
            0});
            this.numFontSize.Minimum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numFontSize.Name = "numFontSize";
            this.numFontSize.Size = new System.Drawing.Size(51, 20);
            this.numFontSize.TabIndex = 3;
            this.numFontSize.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // tabAdvanced
            // 
            this.tabAdvanced.Controls.Add(this.grpLogo);
            this.tabAdvanced.Controls.Add(this.chkShowRawRibbon);
            this.tabAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tabAdvanced.Name = "tabAdvanced";
            this.tabAdvanced.Padding = new System.Windows.Forms.Padding(9);
            this.tabAdvanced.Size = new System.Drawing.Size(386, 251);
            this.tabAdvanced.TabIndex = 2;
            this.tabAdvanced.Text = "Gelişmiş";
            // 
            // grpLogo
            // 
            this.grpLogo.Controls.Add(this.chkIncludeLogo);
            this.grpLogo.Controls.Add(this.lblLogoPath);
            this.grpLogo.Controls.Add(this.txtLogoPath);
            this.grpLogo.Controls.Add(this.btnBrowseLogo);
            this.grpLogo.Controls.Add(this.picLogoPreview);
            this.grpLogo.Location = new System.Drawing.Point(12, 100);
            this.grpLogo.Name = "grpLogo";
            this.grpLogo.Size = new System.Drawing.Size(362, 139);
            this.grpLogo.TabIndex = 2;
            this.grpLogo.TabStop = false;
            this.grpLogo.Text = "Logo Ayarları";
            this.grpLogo.Visible = false;
            // 
            // chkIncludeLogo
            // 
            this.chkIncludeLogo.AutoSize = true;
            this.chkIncludeLogo.Location = new System.Drawing.Point(13, 22);
            this.chkIncludeLogo.Name = "chkIncludeLogo";
            this.chkIncludeLogo.Size = new System.Drawing.Size(73, 17);
            this.chkIncludeLogo.TabIndex = 0;
            this.chkIncludeLogo.Text = "Logo ekle";
            // 
            // lblLogoPath
            // 
            this.lblLogoPath.AutoSize = true;
            this.lblLogoPath.Location = new System.Drawing.Point(13, 48);
            this.lblLogoPath.Name = "lblLogoPath";
            this.lblLogoPath.Size = new System.Drawing.Size(74, 13);
            this.lblLogoPath.TabIndex = 1;
            this.lblLogoPath.Text = "Logo Dosyası:";
            // 
            // txtLogoPath
            // 
            this.txtLogoPath.Location = new System.Drawing.Point(103, 45);
            this.txtLogoPath.Name = "txtLogoPath";
            this.txtLogoPath.ReadOnly = true;
            this.txtLogoPath.Size = new System.Drawing.Size(172, 20);
            this.txtLogoPath.TabIndex = 2;
            // 
            // btnBrowseLogo
            // 
            this.btnBrowseLogo.Location = new System.Drawing.Point(279, 44);
            this.btnBrowseLogo.Name = "btnBrowseLogo";
            this.btnBrowseLogo.Size = new System.Drawing.Size(34, 22);
            this.btnBrowseLogo.TabIndex = 3;
            this.btnBrowseLogo.Text = "...";
            // 
            // picLogoPreview
            // 
            this.picLogoPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLogoPreview.Location = new System.Drawing.Point(103, 74);
            this.picLogoPreview.Name = "picLogoPreview";
            this.picLogoPreview.Size = new System.Drawing.Size(129, 52);
            this.picLogoPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLogoPreview.TabIndex = 4;
            this.picLogoPreview.TabStop = false;
            // 
            // chkShowRawRibbon
            // 
            this.chkShowRawRibbon.AutoSize = true;
            this.chkShowRawRibbon.Location = new System.Drawing.Point(12, 12);
            this.chkShowRawRibbon.Name = "chkShowRawRibbon";
            this.chkShowRawRibbon.Size = new System.Drawing.Size(167, 17);
            this.chkShowRawRibbon.TabIndex = 0;
            this.chkShowRawRibbon.Text = "Kaynak Kod Butonunu Göster";
            this.chkShowRawRibbon.UseVisualStyleBackColor = true;
            this.chkShowRawRibbon.CheckedChanged += new System.EventHandler(this.chkShowRawRibbon_CheckedChanged);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(415, 400);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.pnlFooter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mailify";
            this.Load += new System.EventHandler(this.SettingsForm_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlFooter.ResumeLayout(false);
            this.tabControl.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.grpAlign.ResumeLayout(false);
            this.grpAlign.PerformLayout();
            this.verticalPanel.ResumeLayout(false);
            this.verticalPanel.PerformLayout();
            this.horizontalPanel.ResumeLayout(false);
            this.horizontalPanel.PerformLayout();
            this.grpCompany.ResumeLayout(false);
            this.grpCompany.PerformLayout();
            this.tabAppearance.ResumeLayout(false);
            this.tabAppearance.PerformLayout();
            this.grpFooterText.ResumeLayout(false);
            this.grpFooterText.PerformLayout();
            this.grpColors.ResumeLayout(false);
            this.grpColors.PerformLayout();
            this.grpFont.ResumeLayout(false);
            this.grpFont.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFontSize)).EndInit();
            this.tabAdvanced.ResumeLayout(false);
            this.tabAdvanced.PerformLayout();
            this.grpLogo.ResumeLayout(false);
            this.grpLogo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLogoPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        // Panel ve Konteynerler
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Label lblTitle;

        // Tab Control
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabAppearance;
        private System.Windows.Forms.TabPage tabAdvanced;

        // Genel Ayarlar
        private System.Windows.Forms.GroupBox grpCompany;
        private System.Windows.Forms.Label lblCompanyName;

        // Renk Ayarları
        private System.Windows.Forms.GroupBox grpColors;
        private System.Windows.Forms.Label lblHeaderColor;
        private System.Windows.Forms.TextBox txtHeaderColor;
        private System.Windows.Forms.Panel pnlColorPreview;

        // Font Ayarları
        private System.Windows.Forms.GroupBox grpFont;
        private System.Windows.Forms.Label lblFontFamily;
        private System.Windows.Forms.ComboBox cmbFontFamily;
        private System.Windows.Forms.Label lblFontSize;
        private System.Windows.Forms.NumericUpDown numFontSize;

        // Butonlar
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnReset;

        // Diğer
        private System.Windows.Forms.ToolTip toolTip;
        private ComboBox templateBox;
        private GroupBox grpFooterText;
        private TextBox txtFooterText;
        private Label lblFooterText;
        private CheckBox chkIncludeFooter;
        private CheckBox chkShowAllTemplate;
        private CheckBox chkShowRawRibbon;
        private GroupBox grpLogo;
        private CheckBox chkIncludeLogo;
        private Label lblLogoPath;
        private TextBox txtLogoPath;
        private Button btnBrowseLogo;
        private PictureBox picLogoPreview;
        private GroupBox grpAlign;
        private RadioButton radioHorizontalRight;
        private RadioButton radioHorizontalMid;
        private RadioButton radioHorizontalLeft;
        private RadioButton radioVerticalBottom;
        private RadioButton radioVerticalMid;
        private RadioButton radioVerticalTop;
        private Label label2;
        private Label label1;
        private Panel horizontalPanel;
        private Panel verticalPanel;
    }
}