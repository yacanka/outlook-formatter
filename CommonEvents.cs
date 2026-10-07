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
                    Application app = Globals.ThisAddIn.Application;
                    MailItem newMailItem = app.CreateItem(Outlook.OlItemType.olMailItem);
                    newMailItem.Subject = mailItem.Subject ?? "Konu Yok";

                    if (!string.IsNullOrEmpty(newMailItem.HTMLBody))
                    {
                        newMailItem.HTMLBody = mailItem.HTMLBody;
                    }
                    else
                    {
                        newMailItem.Body = mailItem.Body ?? "";
                    }

                    Utils.AttachmentHelper.CopyAttachmentsPreserveInline(mailItem, newMailItem);

                    newMailItem.HTMLBody = HtmlTemplateHelper.CreateFromTemplate(newMailItem.HTMLBody);
                    newMailItem.Display(false);
                }
                else
                {
                    mailItem.HTMLBody = HtmlTemplateHelper.CreateFromTemplate(mailItem.HTMLBody);

                }

            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Hata oluştu: {ex.Message}\n\n{ex.StackTrace}",
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
            }
        }

        public static void ConvertRawBody(object sender, RibbonControlEventArgs e)
        {
            try
            {
                RibbonToggleButton btn = (RibbonToggleButton)sender;
                MailItem mailItem = GetActiveMail(e);

                if (mailItem == null)
                {
                    btn.Checked = false;
                    return;
                }

                if (btn.Checked)
                {
                    mailItem.Body = mailItem.HTMLBody;
                }
                else
                {
                    mailItem.HTMLBody = mailItem.Body;
                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    $"Hata oluştu: {ex.Message}",
                    "Hata",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
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
