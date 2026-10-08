using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
namespace Microsoft.Office.Interop.Outlook
{
    public enum OlAttachmentType { olByValue = 1 }
    public class MailItem
    {
        public Attachments Attachments { get; } = new Attachments();
        public string HTMLBody { get; set; }
        private string body;
        public string Body { get { return body; } set { body = value; HTMLBody = "<html><body>" + System.Net.WebUtility.HtmlEncode(value) + "</body></html>"; } }
        public string Subject { get; set; }
        public OlBodyFormat BodyFormat { get; set; }
        public bool Displayed;
        public bool Discarded;
        public void Display(bool modal) { Displayed = true; }
        public void Close(OlInspectorClose mode) { Discarded = true; }
    }
    public class PropertyAccessor
    {
        public readonly Dictionary<string, object> Values = new Dictionary<string, object>();
        public bool FailWrites;
        public int? ReadError;
        public object GetProperty(string name)
        {
            if (ReadError.HasValue) throw new COMException("Read failed", ReadError.Value);
            if (!Values.ContainsKey(name)) throw new COMException("Absent", unchecked((int)0x8004010F));
            return Values[name];
        }
        public void SetProperty(string name, object value)
        {
            if (FailWrites) throw new COMException("Write failed", unchecked((int)0x80070005));
            Values[name] = value;
        }
    }
    public class Attachment
    {
        public string FileName { get; set; } = "sample.txt";
        public byte[] Content = new byte[] { 1, 2, 3 };
        public string SavedPath;
        public bool FailSave;
        public PropertyAccessor PropertyAccessor { get; } = new PropertyAccessor();
        public void SaveAsFile(string path)
        {
            SavedPath = path;
            File.WriteAllBytes(path, Content);
            if (FailSave) throw new IOException("Save interrupted");
        }
    }
    public class Attachments
    {
        public readonly List<Attachment> Items = new List<Attachment>();
        public bool FailNewProperties;
        public int Count { get { return Items.Count; } }
        public Attachment this[int index] { get { return Items[index - 1]; } }
        public Attachment Add(object path, OlAttachmentType type, object position, object displayName)
        {
            var attachment = new Attachment { FileName = Path.GetFileName((string)path), Content = File.ReadAllBytes((string)path) };
            attachment.PropertyAccessor.FailWrites = FailNewProperties;
            Items.Add(attachment);
            return attachment;
        }
        public void Remove(int index) { Items.RemoveAt(index - 1); }
    }
}
namespace Microsoft.Office.Interop.Outlook
{
    public enum OlItemType { olMailItem }
    public enum OlBodyFormat { olFormatUnspecified = 0, olFormatPlain = 1, olFormatHTML = 2, olFormatRichText = 3 }
    public enum OlInspectorClose { olDiscard }
    public class Application
    {
        public MailItem Created;
        public MailItem CreateItem(OlItemType type) { Created = new MailItem(); return Created; }
    }
    public class Selection
    {
        public int Count { get; set; }
        public object Selected;
        public object this[int index] { get { return Selected; } }
    }
    public class Explorer { public Selection Selection { get; } = new Selection(); }
    public class Inspector { public object CurrentItem { get; set; } }
}
namespace Microsoft.Office.Tools.Ribbon
{
    public class RibbonControl { public object Context { get; set; } }
    public class RibbonControlEventArgs : System.EventArgs { public RibbonControl Control { get; set; } }
    public class RibbonToggleButton { public bool Checked { get; set; } public bool Visible { get; set; } public void PerformLayout() { } }
}
namespace MailFormatter
{
    public static class Globals { public static AddInDouble ThisAddIn = new AddInDouble(); public static RibbonCollection Ribbons = new RibbonCollection(); }
    public class AddInDouble { public Microsoft.Office.Interop.Outlook.Application Application = new Microsoft.Office.Interop.Outlook.Application(); }
}
#if !REAL_FORMS
namespace MailFormatter.Forms
{
    public class SettingsForm : System.IDisposable { public void ShowDialog() { } public void Dispose() { } }
}
namespace MailFormatter.Forms
{
    public class RawBodyForm : System.IDisposable
    {
        public static string LastSource;
        public RawBodyForm(string source) { LastSource = source; }
        public void ShowDialog() { }
        public void Dispose() { }
    }
}

#endif

namespace MailFormatter
{
    public class RibbonCollection { public T GetRibbon<T>() where T : new() { return new T(); } }
    public class FormatterRibbon { public Microsoft.Office.Tools.Ribbon.RibbonToggleButton btnShowRaw = new Microsoft.Office.Tools.Ribbon.RibbonToggleButton(); }
    public class FormatterRibbonCompose { public Microsoft.Office.Tools.Ribbon.RibbonToggleButton btnShowRaw = new Microsoft.Office.Tools.Ribbon.RibbonToggleButton(); }
}
