using MailFormatter;
using Microsoft.Office.Tools.Ribbon;
using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter
{
    public partial class FormatterRibbon
    {
        private void FormatterRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            // Ribbon yüklendiğinde yapılacak işlemler
        }

        private void btnSettings_Click(object sender, RibbonControlEventArgs e)
        {
            CommonEvents.ShowSettings(sender, e);
        }

        private void btnFormatMail_Click(object sender, RibbonControlEventArgs e)
        {
            CommonEvents.FormatMail(sender, e);
        }

        private void btnShowRaw_Click(object sender, RibbonControlEventArgs e)
        {
            CommonEvents.ConvertRawBody(sender, e);
        }
    }
}