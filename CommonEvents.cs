using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter
{
    public static class CommonEvents
    {
        public static void ShowSettings(object sender, RibbonControlEventArgs e)
        {
            using (var settingsForm = new Forms.SettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }

        public static void FormatMail(object sender, RibbonControlEventArgs e)
        {
            try
            {
                MailItem mailItem = GetActiveMail(e);

                if (mailItem == null) return;

                if (e.Control.Context is Explorer explorer && explorer != null)
                {
                    CreateFormattedCopy(mailItem);
                }
                else
                {
                    string original = mailItem.HTMLBody;
                    string formatted = HtmlTemplateHelper.CreateFromTemplate(original);
                    if (formatted != original) mailItem.HTMLBody = formatted;

                }

            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Mailify operation failed: {0}", ex.GetType().Name);
                System.Windows.Forms.MessageBox.Show(
                    "E-posta biçimlendirilemedi. İşlem tamamlanmadı.",
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static void ConvertRawBody(object sender, RibbonControlEventArgs e)
        {
            try
            {
                MailItem mailItem = GetActiveMail(e);
                if (mailItem == null) return;
                using (var preview = new Forms.RawBodyForm(mailItem.HTMLBody ?? string.Empty))
                    preview.ShowDialog();
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Mailify operation failed: {0}", ex.GetType().Name);
                System.Windows.Forms.MessageBox.Show(
                    $"Hata oluştu: {ex.Message}",
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
            finally
            {
                if (sender is RibbonToggleButton button) button.Checked = false;
            }
        }

        private static void CreateFormattedCopy(MailItem original)
        {
            MailItem copy = null;
            bool displayed = false;
            try
            {
                copy = Globals.ThisAddIn.Application.CreateItem(OlItemType.olMailItem);
                copy.Subject = original.Subject ?? "Konu Yok";
                if (original.BodyFormat != OlBodyFormat.olFormatPlain && !string.IsNullOrEmpty(original.HTMLBody))
                {
                    copy.BodyFormat = OlBodyFormat.olFormatHTML;
                    copy.HTMLBody = original.HTMLBody;
                }
                else
                {
                    copy.Body = original.Body ?? string.Empty;
                    copy.BodyFormat = OlBodyFormat.olFormatHTML;
                }
                string formatted = HtmlTemplateHelper.CreateFromTemplate(copy.HTMLBody);
                Utils.AttachmentHelper.CopyAttachmentsPreserveInline(original, copy);
                copy.HTMLBody = formatted;
                copy.Display(false);
                displayed = true;
            }
            finally
            {
                if (copy != null && !displayed)
                {
                    try { copy.Close(OlInspectorClose.olDiscard); }
                    catch (System.Exception ex) { System.Diagnostics.Trace.TraceWarning("Mailify draft cleanup failed: {0}", ex.GetType().Name); }
                }
                Utils.AttachmentHelper.ReleaseOwnedComObject(copy);
            }
        }

        private static MailItem GetActiveMail(RibbonControlEventArgs e)
        {
            MailItem mailItem = null;

            if (e.Control.Context is Inspector inspector)
            {
                mailItem = inspector.CurrentItem as MailItem;
            }
            else if (e.Control.Context is Explorer explorer && explorer != null && explorer.Selection.Count > 0 && explorer.Selection[1] is MailItem selectedMail)
            {
                mailItem = selectedMail;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show(
                    "Lütfen bir mail öğesi seçin!",
                    "Uyarı",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Warning);
            }

            return mailItem;
        }



    }
}
