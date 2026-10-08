using System;
using System.IO;
using System.Runtime.InteropServices;
using MailFormatter.Utils;
using Microsoft.Office.Interop.Outlook;
internal static class AttachmentTests
{
    static readonly string Cid = "http://schemas.microsoft.com/mapi/proptag/0x3712001F";
    static MailItem Source(out Attachment attachment)
    {
        var mail = new MailItem { HTMLBody = "<html><body><img src='cid:sample'></body></html>" };
        attachment = new Attachment();
        attachment.PropertyAccessor.Values[Cid] = "sample";
        mail.Attachments.Items.Add(attachment);
        return mail;
    }
    public static void Run(Action<string, Action> check)
    {
        check("Inline properties and bytes are preserved", () => {
            Attachment a; var source = Source(out a); var target = new MailItem();
            AttachmentHelper.CopyAttachmentsPreserveInline(source, target);
            if (target.Attachments.Count != 1 || (string)target.Attachments[1].PropertyAccessor.GetProperty(Cid) != "sample" || target.Attachments[1].Content[2] != 3) throw new Exception("Copy incomplete");
            if (Directory.Exists(Path.GetDirectoryName(a.SavedPath))) throw new Exception("Temporary directory leaked");
        });
        check("Property write failure rolls back newly copied attachments", () => {
            Attachment a; var source = Source(out a); var target = new MailItem();
            target.Attachments.Items.Add(new Attachment()); target.Attachments.FailNewProperties = true;
            bool threw = false;
            try { AttachmentHelper.CopyAttachmentsPreserveInline(source, target); } catch(COMException) { threw = true; }
            if (!threw || target.Attachments.Count != 1) throw new Exception("Failure swallowed or partial copy retained");
        });
        check("Non-missing metadata read errors propagate", () => {
            Attachment a; var source = Source(out a); var target = new MailItem();
            a.PropertyAccessor.ReadError = unchecked((int)0x80070005);
            bool threw = false;
            try { AttachmentHelper.CopyAttachmentsPreserveInline(source, target); } catch(COMException) { threw = true; }
            if (!threw || target.Attachments.Count != 0) throw new Exception("Read failure swallowed");
        });
        check("Partial SaveAsFile output is cleaned on failure", () => {
            Attachment a; var source = Source(out a); var target = new MailItem(); a.FailSave = true;
            try { AttachmentHelper.CopyAttachmentsPreserveInline(source, target); } catch(IOException) { }
            if (a.SavedPath == null || Directory.Exists(Path.GetDirectoryName(a.SavedPath))) throw new Exception("Partial file leaked");
        });
    }
}
