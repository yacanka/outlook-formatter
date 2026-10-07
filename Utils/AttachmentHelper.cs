using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter.Utils
{
    public static class AttachmentHelper
    {
        // MAPI Property Tag'leri
        private const string PR_ATTACH_CONTENT_ID = "http://schemas.microsoft.com/mapi/proptag/0x3712001F";
        private const string PR_ATTACH_CONTENT_LOCATION = "http://schemas.microsoft.com/mapi/proptag/0x3713001F";
        private const string PR_ATTACHMENT_HIDDEN = "http://schemas.microsoft.com/mapi/proptag/0x7FFE000B";
        private const string PR_ATTACH_DISPOSITION = "http://schemas.microsoft.com/mapi/proptag/0x3716001F";
        private const string PR_ATTACH_MIME_TAG = "http://schemas.microsoft.com/mapi/proptag/0x370E001F";
        private const string PR_ATTACH_FLAGS = "http://schemas.microsoft.com/mapi/proptag/0x37140003";

        public static string AddInlineImage(Outlook.MailItem mail, Image resourceImage, string imageFormat = "png")
        {
            imageFormat = imageFormat.ToLower();
            string uid = Guid.NewGuid().ToString("N").ToUpper();
            string contentId = $"image.{imageFormat}@{uid.Substring(0, 16)}.{uid.Substring(16, 16)}";

            string tempFile = Path.Combine(Path.GetTempPath(), $"{contentId}.{imageFormat}");

            // Resmi kaydet
            ImageFormat format;
            switch (imageFormat)
            {
                case "jpg":
                case "jpeg":
                    format = ImageFormat.Jpeg;
                    break;
                case "gif":
                    format = ImageFormat.Gif;
                    break;
                case "bmp":
                    format = ImageFormat.Bmp;
                    break;
                default:
                    format = ImageFormat.Png;
                    break;
            }

            resourceImage.Save(tempFile, format);

            if (string.IsNullOrEmpty(tempFile) || !File.Exists(tempFile)) return null;

            Outlook.Attachment attachment = mail.Attachments.Add(
                tempFile,
                Outlook.OlAttachmentType.olByValue,
                0,
                Path.GetFileName(tempFile));

            attachment.PropertyAccessor.SetProperty(
                    PR_ATTACH_CONTENT_ID,
                    contentId);

            attachment.PropertyAccessor.SetProperty(
               PR_ATTACHMENT_HIDDEN,
               true);

            attachment.PropertyAccessor.SetProperty(
                    PR_ATTACH_DISPOSITION,
                    "inline");

            attachment.PropertyAccessor.SetProperty(
                    PR_ATTACH_MIME_TAG,
                    $"image/{imageFormat}");

            return contentId;
        }

        public static void CopyAttachmentsPreserveInline(Outlook.MailItem src, Outlook.MailItem dst)
        {
            if (src.Attachments == null || src.Attachments.Count == 0)
                return;

            string html = src.HTMLBody ?? string.Empty;

            string baseDir = System.IO.Path.Combine(
                System.IO.Path.GetTempPath(),
                "MyAddinMailClone",
                System.Guid.NewGuid().ToString("N")
            );
            System.IO.Directory.CreateDirectory(baseDir);

            for (int i = 1; i <= src.Attachments.Count; i++)
            {
                Outlook.Attachment srcAtt = src.Attachments[i];

                // Kaynak attachment’ın inline bilgisini oku
                var meta = ReadInlineMeta(srcAtt);

                // Dosyaya kaydet → hedefe ekle
                string safeName = MakeSafeFileName(srcAtt.FileName);
                string fullPath = System.IO.Path.Combine(baseDir, safeName);
                srcAtt.SaveAsFile(fullPath);
                Outlook.Attachment dstAtt;
                try
                {
                    dstAtt = dst.Attachments.Add(
                    fullPath,
                    Outlook.OlAttachmentType.olByValue,
                    Type.Missing,
                    Type.Missing
                );
                }
                finally
                {
                    TryDeleteFile(fullPath);
                }


                if (dstAtt == null)
                    continue;

                // Inline olup olmadığını tespit et:
                // - PR_ATTACHMENT_HIDDEN true veya
                // - Content-ID mevcut ve HTML'de cid:contentId geçiyor veya
                // - Content-Location mevcut ve HTML içinde geçiyor
                bool inlineByCid = !string.IsNullOrEmpty(meta.ContentId) && ContainsCid(html, meta.ContentId);
                bool inlineByLoc = !string.IsNullOrEmpty(meta.ContentLocation) && html.IndexOf(meta.ContentLocation, StringComparison.OrdinalIgnoreCase) >= 0;

                bool isInline = meta.Hidden == true || inlineByCid || inlineByLoc;

                // Hedef attachment’a property’leri taşı
                if (!string.IsNullOrEmpty(meta.ContentId))
                    SetStringProp(dstAtt, PR_ATTACH_CONTENT_ID, meta.ContentId);

                if (!string.IsNullOrEmpty(meta.ContentLocation))
                    SetStringProp(dstAtt, PR_ATTACH_CONTENT_LOCATION, meta.ContentLocation);

                if (meta.Flags.HasValue)
                    SetIntProp(dstAtt, PR_ATTACH_FLAGS, meta.Flags.Value);

                if (isInline)
                    SetBoolProp(dstAtt, PR_ATTACHMENT_HIDDEN, true);
            }

            // Bazı Outlook sürümlerinde inline render için HTMLBody’yi sonda tekrar set etmek faydalı olur
            if (!string.IsNullOrEmpty(dst.HTMLBody))
                dst.HTMLBody = dst.HTMLBody;
        }

        private static void TryDeleteFile(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch
            {
                // Pass
            }
        }

        private static bool ContainsCid(string html, string contentId)
        {
            // contentId bazen <...> ile gelir; html’de genelde çıplak olur
            string cid = contentId.Trim();
            if (cid.StartsWith("<") && cid.EndsWith(">"))
                cid = cid.Substring(1, cid.Length - 2);

            // cid:xxx veya cid:<xxx> gibi varyasyonlar
            return html.IndexOf("cid:" + cid, StringComparison.OrdinalIgnoreCase) >= 0
                || html.IndexOf("cid:<" + cid + ">", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string MakeSafeFileName(string name)
        {
            foreach (char c in System.IO.Path.GetInvalidFileNameChars())
                name = name.Replace(c, '_');
            return name;
        }

        private sealed class InlineMeta
        {
            public string ContentId { get; set; }
            public string ContentLocation { get; set; }
            public bool? Hidden { get; set; }
            public int? Flags { get; set; }
        }

        private static InlineMeta ReadInlineMeta(Outlook.Attachment att)
        {
            var meta = new InlineMeta();

            meta.ContentId = GetStringProp(att, PR_ATTACH_CONTENT_ID);
            meta.ContentLocation = GetStringProp(att, PR_ATTACH_CONTENT_LOCATION);
            meta.Hidden = GetBoolProp(att, PR_ATTACHMENT_HIDDEN);
            meta.Flags = GetIntProp(att, PR_ATTACH_FLAGS);

            // Bazı mailler Content-ID’yi <...> şeklinde saklar; normalize edelim
            if (!string.IsNullOrEmpty(meta.ContentId))
            {
                meta.ContentId = meta.ContentId.Trim();
            }

            return meta;
        }

        private static string GetStringProp(Outlook.Attachment att, string schema)
        {
            try
            {
                var pa = att.PropertyAccessor;
                object v = pa.GetProperty(schema);
                return v?.ToString();
            }
            catch
            {
                return null;
            }
        }

        private static int? GetIntProp(Outlook.Attachment att, string schema)
        {
            try
            {
                var pa = att.PropertyAccessor;
                object v = pa.GetProperty(schema);
                if (v == null) return null;
                return Convert.ToInt32(v);
            }
            catch
            {
                return null;
            }
        }

        private static bool? GetBoolProp(Outlook.Attachment att, string schema)
        {
            try
            {
                var pa = att.PropertyAccessor;
                object v = pa.GetProperty(schema);
                if (v == null) return null;
                return Convert.ToBoolean(v);
            }
            catch
            {
                return null;
            }
        }

        private static void SetStringProp(Outlook.Attachment att, string schema, string value)
        {
            try
            {
                att.PropertyAccessor.SetProperty(schema, value);
            }
            catch
            {
                // bazı attachment tiplerinde set engellenebilir
            }
        }

        private static void SetIntProp(Outlook.Attachment att, string schema, int value)
        {
            try
            {
                att.PropertyAccessor.SetProperty(schema, value);
            }
            catch
            {
            }
        }

        private static void SetBoolProp(Outlook.Attachment att, string schema, bool value)
        {
            try
            {
                att.PropertyAccessor.SetProperty(schema, value);
            }
            catch
            {
            }
        }
    }
}
