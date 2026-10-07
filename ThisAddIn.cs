using System;
using Outlook = Microsoft.Office.Interop.Outlook;

namespace MailFormatter
{
    public partial class ThisAddIn
    {
        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            // Eklenti başlatıldığında
            System.Diagnostics.Debug.WriteLine("Mail Formatter Eklentisi yüklendi.");
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {
            // Eklenti kapatıldığında temizlik
        }

        /// <summary>
        /// Seçili mail öğesini döndürür (global erişim için)
        /// </summary>
        public Outlook.MailItem GetCurrentMailItem()
        {
            try
            {
                if (Application.ActiveExplorer()?.Selection.Count > 0)
                {
                    return Application.ActiveExplorer().Selection[1] as Outlook.MailItem;
                }

                if (Application.ActiveInspector()?.CurrentItem is Outlook.MailItem mailItem)
                {
                    return mailItem;
                }
            }
            catch { }

            return null;
        }

        #region VSTO generated code
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }
        #endregion
    }
}