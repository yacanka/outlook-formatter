using System;
using MailFormatter;
using MailFormatter.Properties;

internal static class RegressionTests
{
    private static int failed;
    private static void Check(string name, Action test)
    {
        try { test(); Console.WriteLine("PASS " + name); }
        catch (Exception ex) { failed++; Console.WriteLine("FAIL " + name + ": " + ex.Message); }
    }
    private static void Equal(string expected, string actual)
    {
        if (expected != actual) throw new Exception("Unexpected output");
    }
    private static void Require(bool condition) { if (!condition) throw new Exception("Assertion failed"); }
    public static int Main()
    {
        Settings.Default.TemplateIndex = 0;
        Settings.Default.IncludeFooter = true;
        Check("Null body is preserved", () => Equal(null, HtmlTemplateHelper.CreateFromTemplate(null)));
        Check("HTML fragment is preserved", () => Equal("<p>Keep me</p>", HtmlTemplateHelper.CreateFromTemplate("<p>Keep me</p>")));
        Check("Reversed body tags are preserved", () => Equal("</body><body>Keep me", HtmlTemplateHelper.CreateFromTemplate("</body><body>Keep me")));
        Check("Malformed opening body is preserved", () => Equal("<body broken </body>", HtmlTemplateHelper.CreateFromTemplate("<body broken </body>")));
        Check("Similar tag name is not body", () => Equal("<bodyguard>Keep me</body>", HtmlTemplateHelper.CreateFromTemplate("<bodyguard>Keep me</body>")));
        Check("Invalid template keeps content", () => {
            Settings.Default.TemplateIndex = -1;
            Equal("<html><body>Keep me</body></html>", HtmlTemplateHelper.CreateFromTemplate("<html><body>Keep me</body></html>"));
            Settings.Default.TemplateIndex = 0;
        });
        Check("All templates preserve body and head", () => {
            for (int i = 0; i < 10; i++) {
                Settings.Default.TemplateIndex = i;
                var html = HtmlTemplateHelper.CreateFromTemplate("<html><head><style>.keep{color:red}</style></head><BODY class='keep'><p>Keep me</p></BODY></html>");
                Require(html.Contains("<p>Keep me</p>") && html.Contains(".keep{color:red}"));
            }
        });
        Check("Commented body tags are not formatted", () => {
            Settings.Default.TemplateIndex = 0;
            const string prefix = "<html><!-- <body>example</body> --><body>";
            var html = HtmlTemplateHelper.CreateFromTemplate(prefix + "Actual</body></html>");
            Require(html.StartsWith(prefix) && html.Contains("Actual"));
        });
        Check("Body tags inside quoted attributes are ignored", () => {
            const string prefix = "<html><head><meta content='<body>example</body>'></head><body>";
            var html = HtmlTemplateHelper.CreateFromTemplate(prefix + "Actual</body></html>");
            Require(html.StartsWith(prefix) && html.Contains("Actual"));
        });
        Check("Reply with a quoted formatted message can be formatted", () => {
            var quote = "<a name='MailifyFormattedV1'></a><p>Old message</p>";
            string input = "<html><body><p>New reply</p><blockquote>" + quote + "</blockquote></body></html>";
            var html = HtmlTemplateHelper.CreateFromTemplate(input);
            Require(html != input && html.Contains("<p>New reply</p>"));
        });
        Check("Data-name attribute is not a formatting bookmark", () => {
            string input = "<html><body><a data-name='MailifyFormattedV1'>Actual</a></body></html>";
            Require(HtmlTemplateHelper.CreateFromTemplate(input) != input);
        });
        Check("Repeated formatting is idempotent", () => {
            Settings.Default.TemplateIndex = 0;
            var once = HtmlTemplateHelper.CreateFromTemplate("<html><body><p>Keep me</p></body></html>");
            Equal(once, HtmlTemplateHelper.CreateFromTemplate(once));
        });
        Check("Footer treats markup as text", () => {
            Settings.Default.FooterText = "<b>Text & more</b>\nNext";
            var html = HtmlTemplateHelper.CreateFromTemplate("<html><body>Keep me</body></html>");
            Require(html.Contains("&lt;b&gt;Text &amp; more&lt;/b&gt;<br/>Next"));
            Require(!html.Contains("<b>Text & more</b>"));
        });
        Check("Body attributes may contain greater-than", () => {
            var html = HtmlTemplateHelper.CreateFromTemplate("<html><body title='a > b'><p>Keep me</p></body></html>");
            Require(html.StartsWith("<html><body title='a > b'>") && html != "<html><body title='a > b'><p>Keep me</p></body></html>");
        });
        AttachmentTests.Run(Check);
        CommonEventTests.Run(Check);
        SettingsTests.Run(Check);
#if REAL_RESOURCES
        AssetTests.Run(Check);
#endif
        Console.WriteLine("Failures: " + failed);
        return failed == 0 ? 0 : 1;
    }
}
