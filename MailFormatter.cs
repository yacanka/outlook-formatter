using MailFormatter;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter
{
    public static class MailFormatter
    {
        /// <summary>
        /// Seçili mailden yeni formatlı mail oluşturur
        /// </summary>
        public static void CreateFormattedMail(Outlook.MailItem originalMail)
        {
            Outlook.Application app = Globals.ThisAddIn.Application;
            Outlook.MailItem newMail = app.CreateItem(Outlook.OlItemType.olMailItem);

            try
            {
                // Orijinal mail bilgilerini al
                string originalSubject = originalMail.Subject ?? "Konu Yok";
                string originalBody = originalMail.Body ?? "";
                string originalSender = originalMail.SenderName ?? "Bilinmeyen";
                DateTime receivedTime = originalMail.ReceivedTime;

                string logoPath = GetEmbeddedImagePath();
                string contentId = "logo_image";

                // Resmi mail'e attachment olarak ekle (embedded için)
                Outlook.Attachment attachment = null;
                if (!string.IsNullOrEmpty(logoPath) && File.Exists(logoPath))
                {
                    attachment = newMail.Attachments.Add(
                        logoPath,
                        Outlook.OlAttachmentType.olEmbeddeditem,
                        0,
                        "Logo");

                    // Content-ID ayarla
                    attachment.PropertyAccessor.SetProperty(
                        "http://schemas.microsoft.com/mapi/proptag/0x3712001F",
                        contentId);
                }

                //string topBannerCid = MailImageHelper.AddInlineImage(newMail, Properties.Resources.topBanner);
                //string bottomBannerCid = MailImageHelper.AddInlineImage(newMail, Properties.Resources.bottomBanner);

                // HTML formatında body oluştur
                string htmlBody = HtmlTemplateHelper.CreateHtmlTemplate(contentId, contentId, originalBody);
                //string htmlBody = HtmlTemplateHelper.CreateExHtmlTemplate(
                //    originalSubject,
                //    originalBody,
                //    originalSender,
                //    receivedTime,
                //    contentId,
                //    logoId);

                // Yeni mail özelliklerini ayarla
                newMail.Subject = $"[Formatlanmış] {originalSubject}";
                newMail.BodyFormat = Outlook.OlBodyFormat.olFormatHTML;
                newMail.HTMLBody = htmlBody;

                // Mail penceresini aç
                newMail.Display(false);
            }
            catch (Exception ex)
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(newMail);
                throw new Exception($"Mail oluşturulurken hata: {ex.Message}", ex);
            }
        }

        

        /// <summary>
        /// Logo dosya yolunu alır
        /// </summary>
        public static string GetEmbeddedImagePath()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "MailFormatterLogo.png");

            try
            {
                // ✅ YÖNTEM 1: Resources.resx'den al (EN GÜVENİLİR)
                // "logo" = Resources.resx'de eklediğin resmin Name'i
                Image logoImage = Properties.Resources.logo;

                if (logoImage != null)
                {
                    logoImage.Save(tempPath, ImageFormat.Png);
                    System.Diagnostics.Debug.WriteLine($"✅ Logo kaydedildi: {tempPath}");
                    return tempPath;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ Resources.resx hatası: {ex.Message}");
            }

            // Resources'da bulunamazsa varsayılan oluştur
            return Utils.ImageHelper.CreateDefaultLogo(tempPath);
        }

        
    }
}