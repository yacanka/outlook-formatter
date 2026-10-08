using System.Drawing;
using System.Windows.Forms;

namespace MailFormatter.Forms
{
    /// <summary>Displays a snapshot of the source; never receives or modifies a MailItem.</summary>
    internal sealed class RawBodyForm : Form
    {
        public RawBodyForm(string source)
        {
            Text = "E-posta HTML Kaynağı — Salt Okunur";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(900, 650);
            MinimizeBox = false;
            ShowInTaskbar = false;
            var sourceText = new TextBox
            {
                Dock = DockStyle.Fill,
                Multiline = true,
                ReadOnly = true,
                WordWrap = false,
                ScrollBars = ScrollBars.Both,
                Text = source ?? string.Empty,
                BackColor = SystemColors.Window
            };
            var close = new Button { Text = "Kapat", Dock = DockStyle.Bottom, Height = 32, DialogResult = DialogResult.Cancel };
            Controls.Add(sourceText);
            Controls.Add(close);
            CancelButton = close;
        }
    }
}
