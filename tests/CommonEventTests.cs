using System;
using MailFormatter;
using MailFormatter.Properties;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;
internal static class CommonEventTests
{
    public static void Run(Action<string, Action> check)
    {
        check("Raw source preview never mutates the message", () => {
            var mail = new MailItem { Body = "Plain", HTMLBody = "<html><body><b>Keep</b></body></html>" };
            var button = new RibbonToggleButton { Checked = true };
            CommonEvents.ConvertRawBody(button, new RibbonControlEventArgs { Control = new RibbonControl { Context = new Inspector { CurrentItem = mail } } });
            if (mail.Body != "Plain" || mail.HTMLBody != "<html><body><b>Keep</b></body></html>" || button.Checked) throw new Exception("Preview modified content or left toggle checked");
            if (MailFormatter.Forms.RawBodyForm.LastSource != mail.HTMLBody) throw new Exception("Preview did not receive HTML");
        });
        check("Explorer copies source HTML even when destination is empty", () => {
            Settings.Default.TemplateIndex = 0;
            var source = new MailItem { Body = "plain", HTMLBody = "<html><body><b>Keep bold</b></body></html>", BodyFormat = OlBodyFormat.olFormatHTML };
            var explorer = new Explorer(); explorer.Selection.Count = 1; explorer.Selection.Selected = source;
            CommonEvents.FormatMail(null, new RibbonControlEventArgs { Control = new RibbonControl { Context = explorer } });
            var target = Globals.ThisAddIn.Application.Created;
            if (!target.Displayed || !target.HTMLBody.Contains("<b>Keep bold</b>")) throw new Exception("Source HTML was lost");
            if (source.HTMLBody != "<html><body><b>Keep bold</b></body></html>") throw new Exception("Source was modified");
        });
        check("Rich-text source retains its available HTML formatting", () => {
            var source = new MailItem { Body = "Plain", HTMLBody = "<html><body><b>Rich</b></body></html>", BodyFormat = OlBodyFormat.olFormatRichText };
            var explorer = new Explorer(); explorer.Selection.Count = 1; explorer.Selection.Selected = source;
            CommonEvents.FormatMail(null, new RibbonControlEventArgs { Control = new RibbonControl { Context = explorer } });
            if (!Globals.ThisAddIn.Application.Created.HTMLBody.Contains("<b>Rich</b>")) throw new Exception("Rich formatting lost");
        });
        check("Plain text is copied without interpreting markup", () => {
            var source = new MailItem { Body = "<b>Literal</b>", BodyFormat = OlBodyFormat.olFormatPlain };
            var explorer = new Explorer(); explorer.Selection.Count = 1; explorer.Selection.Selected = source;
            CommonEvents.FormatMail(null, new RibbonControlEventArgs { Control = new RibbonControl { Context = explorer } });
            if (!Globals.ThisAddIn.Application.Created.HTMLBody.Contains("&lt;b&gt;Literal&lt;/b&gt;")) throw new Exception("Plain text treated as markup");
        });
    }
}
