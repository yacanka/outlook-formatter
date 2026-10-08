using MailFormatter.Properties;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MailFormatter.Forms
{
    public partial class SettingsForm : Form
    {
        private bool _hasChanges = false;

        public SettingsForm()
        {
            InitializeComponent();
        }

        #region Form Events

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            LoadFontFamilies();
            LoadSettings();
            SetupTooltips();
            _hasChanges = false;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (_hasChanges)
            {
                var result = MessageBox.Show(
                    "Kaydedilmemiş değişiklikler var. Çıkmak istediğinize emin misiniz?",
                    "Uyarı",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
            base.OnFormClosing(e);
        }

        #endregion

        #region Load Methods

        /// <summary>
        /// Sistem fontlarını combobox'a yükler
        /// </summary>
        private void LoadFontFamilies()
        {
            InstalledFontCollection fonts = new InstalledFontCollection();

            foreach (FontFamily font in fonts.Families)
            {
                cmbFontFamily.Items.Add(font.Name);
            }

            // Varsayılan font seç
            int index = cmbFontFamily.Items.IndexOf("Segoe UI");
            if (index >= 0)
                cmbFontFamily.SelectedIndex = index;
        }

        /// <summary>
        /// Logo önizlemesini yükler
        /// </summary>
        private void LoadLogoPreview()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLogoPath.Text) && File.Exists(txtLogoPath.Text))
                {
                    // Dosyayı kilitlemeden yükle
                    using (var stream = new FileStream(txtLogoPath.Text, FileMode.Open, FileAccess.Read))
                    {
                        picLogoPreview.Image = Image.FromStream(stream);
                    }
                }
                else
                {
                    picLogoPreview.Image = null;
                }
            }
            catch
            {
                picLogoPreview.Image = null;
            }
        }

        /// <summary>
        /// Tooltip'leri ayarlar
        /// </summary>
        private void SetupTooltips()
        {
            toolTip.SetToolTip(templateBox, "Mail oluşturulurken kullanılacak şablon");
            toolTip.SetToolTip(txtFooterText, "Mail alt bilgisinde görünecek metin");
            toolTip.SetToolTip(txtHeaderColor, "HEX renk kodu (örn: #667eea)");
            toolTip.SetToolTip(chkIncludeLogo, "Mail'e logo resmi eklensin mi?");
            toolTip.SetToolTip(chkShowRawRibbon, "Maillerin kaynak kodunu görüntüleyebilen bir buton ekler");
        }

        #endregion

        #region Button Events

        /// <summary>
        /// Kaydet butonu
        /// </summary>
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateSettings()) return;
            try
            {
                SaveSettings();
            }
            catch (Exception ex)
            {
                _hasChanges = true;
                System.Diagnostics.Trace.TraceError("Mailify settings save failed: {0}", ex.GetType().Name);
                MessageBox.Show("Ayarlar kaydedilemedi. Önceki ayarlar korundu; tekrar deneyebilirsiniz.",
                    "Kayıt Hatası", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _hasChanges = false;
            try
            {
                ShowRawRibbonButton();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceWarning("Mailify ribbon refresh failed: {0}", ex.GetType().Name);
                MessageBox.Show("Ayarlar kaydedildi. Şerit görünümünün güncellenmesi için Outlook'u yeniden açın.",
                    "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// İptal butonu
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Sıfırla butonu
        /// </summary>
        private void btnReset_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(
                "Tüm ayarlar varsayılan değerlere sıfırlanacak. Devam etmek istiyor musunuz?",
                "Sıfırla",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                ResetToDefaults();
            }
        }

        /// <summary>
        /// Logo seç butonu
        /// </summary>
        private void btnBrowseLogo_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Logo Seçin";
                dialog.Filter = "Resim Dosyaları|*.png;*.jpg;*.jpeg;*.gif;*.bmp|Tüm Dosyalar|*.*";
                dialog.FilterIndex = 1;

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtLogoPath.Text = dialog.FileName;
                    LoadLogoPreview();
                    _hasChanges = true;
                }
            }
        }

        /// <summary>
        /// Renk seç butonu
        /// </summary>
        private void btnPickColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog dialog = new ColorDialog())
            {
                dialog.AllowFullOpen = true;
                dialog.AnyColor = true;
                dialog.FullOpen = true;

                // Mevcut rengi yükle
                try
                {
                    dialog.Color = ColorTranslator.FromHtml(txtHeaderColor.Text);
                }
                catch { }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    txtHeaderColor.Text = ColorTranslator.ToHtml(dialog.Color);
                    UpdateColorPreview();
                    _hasChanges = true;
                }
            }
        }

        #endregion

        #region Control Events

        /// <summary>
        /// Logo checkbox değiştiğinde
        /// </summary>
        private void chkIncludeLogo_CheckedChanged(object sender, EventArgs e)
        {
            UpdateLogoControlsState();
            _hasChanges = true;
        }

        /// <summary>
        /// Renk textbox değiştiğinde
        /// </summary>
        private void txtHeaderColor_TextChanged(object sender, EventArgs e)
        {
            UpdateColorPreview();
            _hasChanges = true;
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Logo kontrollerinin durumunu günceller
        /// </summary>
        private void UpdateLogoControlsState()
        {
            bool enabled = chkIncludeLogo.Checked;
            txtLogoPath.Enabled = enabled;
            btnBrowseLogo.Enabled = enabled;
            picLogoPreview.Enabled = enabled;
        }

        /// <summary>
        /// Kaynak kodu gösterme butonunu aktifleştir/pasifleştir
        /// </summary>
        private void ShowRawRibbonButton()
        {
            FormatterRibbon ribbon = Globals.Ribbons.GetRibbon<FormatterRibbon>();
            FormatterRibbonCompose ribbonCompose = Globals.Ribbons.GetRibbon<FormatterRibbonCompose>();

            if (ribbon != null)
            {
                ribbon.btnShowRaw.Visible = Settings.Default.ShowRawRibbonButton;
                ribbon.btnShowRaw.PerformLayout();
            }
            if (ribbonCompose != null)
            {
                ribbonCompose.btnShowRaw.Visible = Settings.Default.ShowRawRibbonButton;
                ribbonCompose.btnShowRaw.PerformLayout();
            }
        }

        /// <summary>
        /// Footer kontrollerinin durumunu günceller
        /// </summary>
        private void UpdateFooterControlsState()
        {
            bool enabled = chkIncludeFooter.Checked;
            grpFooterText.Enabled = enabled;
            grpColors.Enabled = enabled;
            grpFont.Enabled = enabled;
        }

        /// <summary>
        /// Şablon listesini günceller
        /// </summary>
        private void UpdateTemplateListState()
        {
            object prevSelection = templateBox.SelectedItem;

            templateBox.BeginUpdate();

            bool enabled = chkShowAllTemplate.Checked;
            if (enabled)
            {
                foreach (var item in otherTemplateItems)
                {
                    if (!templateBox.Items.Contains(item))
                    {
                        templateBox.Items.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in otherTemplateItems)
                {
                    if (templateBox.Items.Contains(item))
                    {
                        templateBox.Items.Remove(item);
                    }
                }
            }

            templateBox.EndUpdate();

            if (prevSelection != null && templateBox.Items.Contains(prevSelection))
            {
                templateBox.SelectedItem = prevSelection;
            }
            else
            {
                templateBox.SelectedIndex = templateBox.Items.Count > 0 ? 0 : -1;
            }
        }

        /// <summary>
        /// Renk önizlemesini günceller
        /// </summary>
        private void UpdateColorPreview()
        {
            try
            {
                if (!string.IsNullOrEmpty(txtHeaderColor.Text))
                {
                    Color color = ColorTranslator.FromHtml(txtHeaderColor.Text);
                    pnlColorPreview.BackColor = color;
                }
            }
            catch
            {
                pnlColorPreview.BackColor = SystemColors.Control;
            }
        }

        private void RestoreRadioGroupSelection(Control container, string savedTag)
        {
            var radios = container.Controls.OfType<RadioButton>().ToList();
            if (radios.Count == 0)
            {
                return;
            }

            var match = radios.FirstOrDefault(r => (r.Tag?.ToString() ?? "") == (savedTag ?? ""));
            if (match != null)
            {
                match.Checked = true;
                return;
            }

            radios[0].Checked = true;
        }

        /// <summary>
        /// Ayarları doğrular
        /// </summary>
        private bool ValidateSettings()
        {
            if (templateBox.SelectedIndex < 0 || templateBox.SelectedIndex > 9)
            {
                MessageBox.Show("Lütfen geçerli bir şablon seçin.", "Doğrulama Hatası", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Renk formatı kontrolü
            try
            {
                if (!string.IsNullOrEmpty(txtHeaderColor.Text))
                {
                    ColorTranslator.FromHtml(txtHeaderColor.Text);
                }
            }
            catch
            {
                MessageBox.Show(
                    "Geçersiz renk kodu! Örnek format: #667eea",
                    "Doğrulama Hatası",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                txtHeaderColor.Focus();
                return false;
            }

            // Logo dosyası kontrolü
            if (chkIncludeLogo.Checked && !string.IsNullOrEmpty(txtLogoPath.Text))
            {
                if (!File.Exists(txtLogoPath.Text))
                {
                    MessageBox.Show(
                        "Belirtilen logo dosyası bulunamadı!",
                        "Doğrulama Hatası",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Kayıtlı ayarları yükler
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                // Genel Ayarlar
                txtFooterText.Text = Settings.Default.FooterText;

                // Logo Ayarları
                chkShowRawRibbon.Checked = Settings.Default.ShowRawRibbonButton;
                chkIncludeLogo.Checked = Settings.Default.IncludeLogo;
                chkIncludeFooter.Checked = Settings.Default.IncludeFooter;
                chkShowAllTemplate.Checked = Settings.Default.ShowAllTemplates;
                int templateIndex = Settings.Default.TemplateIndex;
                templateBox.SelectedIndex = templateIndex >= 0 && templateIndex < templateBox.Items.Count ? templateIndex : 0;

                txtLogoPath.Text = Settings.Default.LogoPath;

                LoadLogoPreview();
                UpdateFooterControlsState();
                UpdateLogoControlsState();

                // Görünüm Ayarları
                txtHeaderColor.Text = Settings.Default.FooterColor;
                UpdateColorPreview();

                RestoreRadioGroupSelection(horizontalPanel, Settings.Default.HorizontalAlign);
                RestoreRadioGroupSelection(verticalPanel, Settings.Default.VerticalAlign);

                // Font Ayarları
                int fontIndex = cmbFontFamily.Items.IndexOf(Settings.Default.FooterFontFamily);
                if (fontIndex >= 0)
                    cmbFontFamily.SelectedIndex = fontIndex;

                numFontSize.Value = Math.Max(numFontSize.Minimum, Math.Min(numFontSize.Maximum, Settings.Default.FooterFontSize));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ayarlar yüklenirken hata: {ex.Message}",
                    "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Ayarları kaydeder
        /// </summary>
        private void SaveSettings()
        {
            Utils.SettingsPersistence.Save(Settings.Default, () =>
            {
                Settings.Default.ShowRawRibbonButton = chkShowRawRibbon.Checked;
                Settings.Default.TemplateIndex = templateBox.SelectedIndex;
                Settings.Default.IncludeLogo = chkIncludeLogo.Checked;
                Settings.Default.LogoPath = txtLogoPath.Text;
                Settings.Default.IncludeFooter = chkIncludeFooter.Checked;
                Settings.Default.ShowAllTemplates = chkShowAllTemplate.Checked;
                Settings.Default.FooterText = txtFooterText.Text;
                Settings.Default.FooterColor = txtHeaderColor.Text;
                Settings.Default.FooterFontFamily = cmbFontFamily.SelectedItem?.ToString() ?? "Segoe UI";
                Settings.Default.FooterFontSize = (int)numFontSize.Value;

                Settings.Default.HorizontalAlign = (string)horizontalPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Tag;
                Settings.Default.VerticalAlign = (string)verticalPanel.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked)?.Tag;

            });
        }

        /// <summary>
        /// Varsayılan değerlere sıfırlar
        /// </summary>
        private void ResetToDefaults()
        {
            txtFooterText.Text = $"Mailify © {DateTime.Now.Year}";
            chkIncludeLogo.Checked = true;
            chkIncludeFooter.Checked = true;
            txtLogoPath.Text = "";
            txtHeaderColor.Text = "#0255A3";
            chkShowAllTemplate.Checked = false;
            chkShowRawRibbon.Checked = false;

            int fontIndex = cmbFontFamily.Items.IndexOf("Segoe UI");
            if (fontIndex >= 0) cmbFontFamily.SelectedIndex = fontIndex;

            numFontSize.Value = 10;

            RestoreRadioGroupSelection(horizontalPanel, "left");
            RestoreRadioGroupSelection(verticalPanel, "top");

            UpdateColorPreview();
            UpdateLogoControlsState();
            UpdateFooterControlsState();
            picLogoPreview.Image = null;

            _hasChanges = true;
        }

        #endregion

        private void templateBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _hasChanges = true;
        }

        private void chkIncludeFooter_CheckedChanged(object sender, EventArgs e)
        {
            _hasChanges = true;
            UpdateFooterControlsState();
        }

        private void chkShowAllTemplate_CheckedChanged(object sender, EventArgs e)
        {
            _hasChanges = true;
            UpdateTemplateListState();
        }

        private void chkShowRawRibbon_CheckedChanged(object sender, EventArgs e)
        {
            _hasChanges = true;
        }
    }
}