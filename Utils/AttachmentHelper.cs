using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter.Utils
{
    public static class AttachmentHelper
    {
        private const string PR_ATTACH_CONTENT_ID = "http://schemas.microsoft.com/mapi/proptag/0x3712001F";
        private const string PR_ATTACH_CONTENT_LOCATION = "http://schemas.microsoft.com/mapi/proptag/0x3713001F";
        private const string PR_ATTACHMENT_HIDDEN = "http://schemas.microsoft.com/mapi/proptag/0x7FFE000B";
        private const string PR_ATTACH_DISPOSITION = "http://schemas.microsoft.com/mapi/proptag/0x3716001F";
        private const string PR_ATTACH_MIME_TAG = "http://schemas.microsoft.com/mapi/proptag/0x370E001F";
        private const string PR_ATTACH_FLAGS = "http://schemas.microsoft.com/mapi/proptag/0x37140003";
        private const int MapiPropertyNotFound = unchecked((int)0x8004010F);

        public static string AddInlineImage(Outlook.MailItem mail, Image resourceImage, string imageFormat = "png")
        {
            if (mail == null) throw new ArgumentNullException(nameof(mail));
            if (resourceImage == null) throw new ArgumentNullException(nameof(resourceImage));
            imageFormat = (imageFormat ?? "png").ToLowerInvariant();
            if (imageFormat == "jpg") imageFormat = "jpeg";
            ImageFormat format;
            switch (imageFormat)
            {
                case "jpeg": format = ImageFormat.Jpeg; break;
                case "gif": format = ImageFormat.Gif; break;
                case "bmp": format = ImageFormat.Bmp; break;
                case "png": format = ImageFormat.Png; break;
                default: throw new ArgumentException("Desteklenmeyen resim biçimi.", nameof(imageFormat));
            }

            string contentId = Guid.NewGuid().ToString("N") + "@mailify";
            string directory = CreateTemporaryDirectory();
            Outlook.Attachments attachments = null;
            Outlook.Attachment attachment = null;
            int originalCount = -1;
            try
            {
                string file = Path.Combine(directory, "image." + imageFormat);
                resourceImage.Save(file, format);
                attachments = mail.Attachments;
                originalCount = attachments.Count;
                attachment = attachments.Add(file, Outlook.OlAttachmentType.olByValue, 0, Path.GetFileName(file));
                SetProperty(attachment, PR_ATTACH_CONTENT_ID, contentId);
                SetProperty(attachment, PR_ATTACHMENT_HIDDEN, true);
                SetProperty(attachment, PR_ATTACH_DISPOSITION, "inline");
                SetProperty(attachment, PR_ATTACH_MIME_TAG, "image/" + imageFormat);
                return contentId;
            }
            catch
            {
                RollbackAttachments(attachments, originalCount);
                throw;
            }
            finally
            {
                ReleaseOwnedComObject(attachment);
                ReleaseOwnedComObject(attachments);
                DeleteTemporaryDirectory(directory);
            }
        }

        /// <summary>
        /// Copies attachments and inline metadata. Only absent optional MAPI properties are
        /// ignored. Other failures propagate and newly added attachments are rolled back.
        /// The caller must discard a newly created draft if copying fails.
        /// </summary>
        public static void CopyAttachmentsPreserveInline(Outlook.MailItem src, Outlook.MailItem dst)
        {
            if (src == null) throw new ArgumentNullException(nameof(src));
            if (dst == null) throw new ArgumentNullException(nameof(dst));
            if (ReferenceEquals(src, dst)) throw new ArgumentException("Kaynak ve hedef farklı olmalı.");
            Outlook.Attachments source = null;
            Outlook.Attachments target = null;
            string directory = null;
            int originalCount = -1;
            try
            {
                source = src.Attachments;
                int count = source.Count;
                if (count == 0) return;
                target = dst.Attachments;
                originalCount = target.Count;
                directory = CreateTemporaryDirectory();
                string html = src.HTMLBody ?? string.Empty;
                for (int index = 1; index <= count; index++)
                    CopyAttachment(source, target, index, directory, html);
            }
            catch
            {
                RollbackAttachments(target, originalCount);
                throw;
            }
            finally
            {
                ReleaseOwnedComObject(source);
                ReleaseOwnedComObject(target);
                DeleteTemporaryDirectory(directory);
            }
        }

        private static void CopyAttachment(Outlook.Attachments source, Outlook.Attachments target, int index, string directory, string html)
        {
            Outlook.Attachment original = null;
            Outlook.Attachment copy = null;
            try
            {
                original = source[index];
                string contentId = GetOptionalProperty(original, PR_ATTACH_CONTENT_ID) as string;
                string location = GetOptionalProperty(original, PR_ATTACH_CONTENT_LOCATION) as string;
                object hidden = GetOptionalProperty(original, PR_ATTACHMENT_HIDDEN);
                object flags = GetOptionalProperty(original, PR_ATTACH_FLAGS);
                string itemDirectory = Path.Combine(directory, index.ToString(System.Globalization.CultureInfo.InvariantCulture));
                Directory.CreateDirectory(itemDirectory);
                string file = Path.Combine(itemDirectory, MakeSafeFileName(original.FileName));
                original.SaveAsFile(file);
                copy = target.Add(file, Outlook.OlAttachmentType.olByValue, Type.Missing, Type.Missing);
                if (copy == null) throw new InvalidOperationException("Ek dosya kopyalanamadı.");
                if (!string.IsNullOrEmpty(contentId)) SetProperty(copy, PR_ATTACH_CONTENT_ID, contentId);
                if (!string.IsNullOrEmpty(location)) SetProperty(copy, PR_ATTACH_CONTENT_LOCATION, location);
                if (flags != null) SetProperty(copy, PR_ATTACH_FLAGS, Convert.ToInt32(flags));
                bool isInline = (hidden != null && Convert.ToBoolean(hidden)) || ContainsCid(html, contentId) ||
                    (!string.IsNullOrEmpty(location) && html.IndexOf(location, StringComparison.OrdinalIgnoreCase) >= 0);
                if (isInline) SetProperty(copy, PR_ATTACHMENT_HIDDEN, true);
            }
            finally
            {
                ReleaseOwnedComObject(copy);
                ReleaseOwnedComObject(original);
            }
        }

        private static object GetOptionalProperty(Outlook.Attachment attachment, string schema)
        {
            Outlook.PropertyAccessor accessor = null;
            try
            {
                accessor = attachment.PropertyAccessor;
                return accessor.GetProperty(schema);
            }
            catch (COMException ex) when (ex.ErrorCode == MapiPropertyNotFound)
            {
                return null;
            }
            finally { ReleaseOwnedComObject(accessor); }
        }

        private static void SetProperty(Outlook.Attachment attachment, string schema, object value)
        {
            Outlook.PropertyAccessor accessor = null;
            try
            {
                accessor = attachment.PropertyAccessor;
                accessor.SetProperty(schema, value);
            }
            finally { ReleaseOwnedComObject(accessor); }
        }

        private static bool ContainsCid(string html, string contentId)
        {
            if (string.IsNullOrWhiteSpace(contentId)) return false;
            string cid = contentId.Trim().Trim('<', '>');
            return html.IndexOf("cid:" + cid, StringComparison.OrdinalIgnoreCase) >= 0 ||
                html.IndexOf("cid:<" + cid + ">", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string MakeSafeFileName(string name)
        {
            name = Path.GetFileName(name ?? string.Empty);
            foreach (char invalid in Path.GetInvalidFileNameChars()) name = name.Replace(invalid, '_');
            name = name.TrimEnd(' ', '.');
            return string.IsNullOrWhiteSpace(name) ? "attachment.bin" : name;
        }

        private static string CreateTemporaryDirectory()
        {
            string directory = Path.Combine(Path.GetTempPath(), "Mailify", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(directory);
            return directory;
        }

        private static void RollbackAttachments(Outlook.Attachments attachments, int originalCount)
        {
            if (attachments == null || originalCount < 0) return;
            try
            {
                for (int index = attachments.Count; index > originalCount; index--) attachments.Remove(index);
            }
            catch (Exception ex) { Trace.TraceWarning("Mailify attachment rollback failed: {0}", ex.GetType().Name); }
        }

        private static void DeleteTemporaryDirectory(string directory)
        {
            if (directory == null) return;
            try { Directory.Delete(directory, true); }
            catch (IOException ex) { Trace.TraceWarning("Mailify temporary cleanup failed: {0}", ex.GetType().Name); }
            catch (UnauthorizedAccessException ex) { Trace.TraceWarning("Mailify temporary cleanup failed: {0}", ex.GetType().Name); }
        }

        internal static void ReleaseOwnedComObject(object value)
        {
            // Release only wrappers acquired by this operation; never FinalRelease shared Outlook objects.
            if (value != null && Marshal.IsComObject(value)) Marshal.ReleaseComObject(value);
        }
    }
}
