using MailFormatter.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace MailFormatter
{
    public enum NetworkType
    {
        Red,
        Black
    }

    public static class HtmlTemplateHelper
    {
        public static string CreateHtmlTemplate(string topBannerCid, string bottomBannerCid, string originalBody)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($@"<!DOCTYPE html>
<html>

<head>
  <meta charset='utf-8' />
  <meta http-equiv='x-ua-compatible' content='ie=edge' />
  <meta name='viewport' content='width=device-width, initial-scale=1' />
</head>

<body style='margin:0; padding:0; background:#f3f3f3;'>
  <!-- Outer container -->
  <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%'
    style='background:#ffffff; padding:24px 0;'>
    <tr>
      <td align='center' style='padding: 0'>

        <! -- Outer outline box -->
        <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%'
          style='background:#2b2b2b; border: 4px solid #2b2b2b; border-collapse:collapse; min-width: 640px; max-width:1280px;'>
            <tr>
              <td style='padding-right: 0px;'>
                <!-- Frame -->
                <table role='presentation' cellpadding='0' cellspacing='0' border='0' width='100%'
                  style='background:#ffffff; border: 8px solid #ffffff; border-collapse:collapse; min-width: 640px; max-width:1280px;'>
                
                  <!-- TOP BANNER -->
                  <tr>
                    <td style='padding:0; margin:0;'>
                      <img src='cid:{topBannerCid}' width='100%' alt='Top Banner' draggable='false'
                        style='display:block; pointer-events: none; user-select: none;' />
                    </td>
                  </tr>
                
                  <!-- BODY AREA -->
                  <tr>
                    <td
                      style='padding:16px 24px 8px 24px; font-size:14px; line-height:20px; color:#222222;'>
                      <!-- BODY CONTENT START -->
                
                      <div style='margin:0 0 12px 0; white-space: pre-wrap; line-height: 1.6; font-family:Calibri, Arial, sans-serif;'>
                        {EscapeHtml(originalBody)}
                      </div>
                
                      <!-- BODY CONTENT END -->
                    </td>
                  </tr>
                
                  <!-- BOTTOM BANNER -->
                  <tr>
                    <td style='padding:0; margin:0;'>
                      <img src='cid:{bottomBannerCid}' width='100%' alt='Bottom Banner' draggable='false'
                        style='display:block; pointer-events: none; user-select: none;' />
                    </td>
                  </tr>
                
                  <!-- FOOTER TEXT (optional) -->
                  <tr>
                    <td
                      style='padding:8px 16px 4px 16px; font-family:Calibri, Arial, sans-serif; font-size:11px; line-height:16px; color:#666666;'>
                      Bu e-posta formatlanmıştır.
                    </td>
                  </tr>
                </table>
                <!-- /Frame -->
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</body>
<style>

</style>

</html>");
            return sb.ToString();
        }

        public static string CreateFromTemplate(string htmlBody)
        {
            // 1. <body> başlangıcını ve sonunu bul
            int bodyStartIdx = htmlBody.IndexOf("<body", StringComparison.OrdinalIgnoreCase);
            int bodyCloseTagStartIdx = htmlBody.IndexOf("</body>", StringComparison.OrdinalIgnoreCase);

            string finalHtml = string.Empty;

            if (bodyStartIdx != -1 && bodyCloseTagStartIdx != -1)
            {
                // 2. <body ...> etiketinin tam bittiği '>' karakterini bul (Attribute'lar olabilir)
                int bodyTagOpenEndIdx = htmlBody.IndexOf(">", bodyStartIdx) + 1;

                // 3. Mevcut Body içeriğini (etiketler hariç) ayır
                string originalBodyContent = htmlBody.Substring(bodyTagOpenEndIdx, bodyCloseTagStartIdx - bodyTagOpenEndIdx);

                string template = string.Empty;
                int templateIndex = Properties.Settings.Default.TemplateIndex;

                switch (templateIndex)
                {
                    case 0:
                        template = AWNotice(originalBodyContent);
                        break;
                    case 1:
                        template = TemplateBasic(originalBodyContent, Resources.academyDirectorshipBanner, Resources.bottomBannerGray);
                        break;
                    case 2:
                        template = TemplateBasic(originalBodyContent, Resources.appointmentAnnouncement, Resources.bottomBannerGradient);
                        break;
                    case 3:
                        template = TemplateBasic(originalBodyContent, Resources.foreignTradeAndSupportServices, Resources.bottomBannerGreen);
                        break;
                    case 4:
                        template = TemplateBasic(originalBodyContent, Resources.humanResourceAndTalentManagementDirectorship, Resources.bottomBannerGray);
                        break;
                    case 5:
                        template = TemplateBasic(originalBodyContent, Resources.corporateSecurityAndEmergencyDirectorship, Resources.bottomBannerGray);
                        break;
                    case 6:
                        template = TemplateBasic(originalBodyContent, Resources.corporateSolutionsDirectorship, Resources.bottomBannerNavyblue);
                        break;
                    case 7:
                        template = ServiceInterruption(originalBodyContent, NetworkType.Red);
                        break;
                    case 8:
                        template = ServiceInterruption(originalBodyContent, NetworkType.Black);
                        break;
                    case 9:
                        template = TemplateBasic(originalBodyContent, Resources.procurementAndIndustrializationDirectorship, Resources.bottomBannerPurple);
                        break;
                    default:
                        break;
                }

                if (Settings.Default.IncludeFooter)
                {
                    template = AddFooter(template);
                }

                // 6. Tüm HTML'i yeniden oluştur
                // Body öncesi + Yeni İçerik + Body sonrası
                finalHtml = htmlBody.Substring(0, bodyTagOpenEndIdx) +
                                   template +
                                   htmlBody.Substring(bodyCloseTagStartIdx);
            }

            return finalHtml;
        }

        public static string CreateHtmlTemplateV2(string originalBody)
        {
            var settings = Properties.Settings.Default;

            string topBannerSrc = Utils.ImageHelper.ImageToBase64(Properties.Resources.topBannerAWNotice);
            string bottomBannerSrc = Utils.ImageHelper.ImageToBase64(Properties.Resources.bottomBannerAWNotice);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"
<table width='100%' style='padding:24px 18px;' >
<tr>
<td align='center' >
  <table role='presentation' width='1100' bgcolor='black' style='padding: 4px'>
    <tr>
      <td align='center' style='padding-right: 4px;'>
        <table width='1100' bgcolor='white' style='padding: 8px;'>
          <tr>
            <td>
              <table width='1100' bgcolor='white'>
                <!-- TOP BANNER -->
                <tr>
                  <td>
                    <img src='{topBannerSrc}' width='100%' alt='Top Banner' style='display:block; width: 100%' />
                  </td>
                </tr>
              
                <!-- BODY AREA -->
                <tr>
                  <td
                    style='padding:16px 24px 8px 24px; font-size:{settings.FooterFontSize}px; line-height:20px; color:#222222;'>
                    <!-- BODY CONTENT START -->
                      {originalBody}
                    <!-- BODY CONTENT END -->
                  </td>
                </tr>
              
                <!-- BOTTOM BANNER -->
                <tr>
                  <td>
                    <img src='{bottomBannerSrc}' width='100%' alt='Bottom Banner' style='display:block; width: 100%' />
                  </td>
                </tr>
              
                <!-- FOOTER TEXT (optional) -->
                <tr>
                  <td
                    style='padding:8px 16px 4px 16px; font-family:{settings.FooterFontFamily}; font-size:{settings.FooterFontSize}px; line-height:16px; color:{settings.FooterColor};'>
                      {settings.FooterText}
                  </td>
                </tr>
              </table>
            </td>
          </tr>
        </table>
      </td>
    </tr>
  </table>
</td>
</tr>
</table>
");
            return sb.ToString();
        }

        private static string ServiceInterruption(string originalBody, NetworkType type)
        {
            var settings = Settings.Default;

            string bottomBanner = Utils.ImageHelper.ImageToBase64(Properties.Resources.interruptionBottomBanner);
            string topBanner = string.Empty;
            switch (type)
            {
                case NetworkType.Red:
                    topBanner = Utils.ImageHelper.ImageToBase64(Properties.Resources.interruptionTopBannerRed);
                    break;
                case NetworkType.Black:
                    topBanner = Utils.ImageHelper.ImageToBase64(Properties.Resources.interruptionTopBannerBlack);
                    break;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($@"
<div class=WordSection1>
    <div align=center>
        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1067 style='width:800.05pt'>
            <tr>
                <td width=""100%"" valign=top style='width:100.0%;background:#E2261B;padding:0cm 0cm 0cm 0cm'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=1 cellpadding=0 width=1000
                            style='width:750.0pt'>
                            <tr style='height:227.7pt'>
                                <td style='background:white;padding:0cm 0cm 0cm 0cm;height:227.7pt'>
                                    <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1000
                                        style='width:750.0pt'>
                                        <tr>
                                            <td valign=top style='background:#E2261B;padding:0cm 0cm 0cm 0cm'>
                                                <p class=MsoNormal align=right
                                                    style='text-align:right;line-height:115%'><a
                                                        name=""_Hlk217561797""><img width=1061 height=154
                                                            style='width:11.052in;height:1.6041in' id=""_x0000_i1028""
                                                            src=""{topBanner}"">
                                                        <o:p></o:p>
                                                    </a></p>
                                            </td><span style='mso-bookmark:_Hlk217561797'></span>
                                        </tr>
                                        <tr>
                                            <td style='padding:0cm 0cm 0cm 0cm'>
                                                <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
                                                    width=1000 style='width:750.0pt'>
                                                    <tr>
                                                        <td style='padding:0cm 0cm 0cm 0cm'>
                                                            <table class=MsoNormalTable border=0 cellspacing=10
                                                                cellpadding=0 width=1000 style='width:750.0pt'>
                                                                <tr>
                                                                    <td style='padding:7.5pt 7.5pt 7.5pt 7.5pt'>
                                                                        <div align=center>
                                                                            <table class=MsoNormalTable border=1
                                                                                cellspacing=0 cellpadding=0
                                                                                width=1023
                                                                                style='width:766.95pt;border:solid windowtext 1.0pt'>
                                                                                <tr>
                                                                                    <td width=1023
                                                                                        style='width:766.95pt;border:none;padding:0cm 0cm 0cm 0cm'>
                                                                                        <table class=MsoNormalTable
                                                                                            border=0 cellspacing=0
                                                                                            cellpadding=0 width=1000
                                                                                            style='width:750.0pt'>
                                                                                            <tr>
                                                                                                <td
                                                                                                    style='padding:0cm 0cm 0cm 0cm'>
                                                                                                    <table
                                                                                                        class=MsoNormalTable
                                                                                                        border=0
                                                                                                        cellspacing=10
                                                                                                        cellpadding=0
                                                                                                        width=1000
                                                                                                        style='width:750.0pt'>
                                                                                                        <tr
                                                                                                            style=''>
                                                                                                            <td align='{settings.HorizontalAlign}' valign='{settings.VerticalAlign}' 
                                                                                                                style='padding:7.5pt 7.5pt 7.5pt 7.5pt;'>
                                                                                                                {originalBody}
                                                                                                            </td>
                                                                                                            <span
                                                                                                                style='mso-bookmark:_Hlk217561797'></span>
                                                                                                        </tr>
                                                                                                    </table><span
                                                                                                        style='mso-bookmark:_Hlk217561797'></span>
                                                                                                </td><span
                                                                                                    style='mso-bookmark:_Hlk217561797'></span>
                                                                                            </tr>
                                                                                        </table><span
                                                                                            style='mso-bookmark:_Hlk217561797'></span>
                                                                                    </td><span
                                                                                        style='mso-bookmark:_Hlk217561797'></span>
                                                                                </tr>
                                                                            </table>
                                                                        </div><span
                                                                            style='mso-bookmark:_Hlk217561797'></span>
                                                                    </td><span
                                                                        style='mso-bookmark:_Hlk217561797'></span>
                                                                </tr>
                                                            </table><span style='mso-bookmark:_Hlk217561797'></span>
                                                        </td><span style='mso-bookmark:_Hlk217561797'></span>
                                                    </tr>
                                                </table><span style='mso-bookmark:_Hlk217561797'></span>
                                            </td><span style='mso-bookmark:_Hlk217561797'></span>
                                        </tr>
                                        <tr>
                                            <td style='background:#E2261B;padding:0cm 0cm 0cm 0cm'>
                                                <p class=MsoNormal style='line-height:115%'><span
                                                        style='mso-bookmark:_Hlk217561797'><span
                                                            style='color:black'><img width=1065 height=153
                                                                style='width:11.0937in;height:1.5937in'
                                                                id=""Picture_x0020_1""
                                                                src=""{bottomBanner}""></span></span><span
                                                        style='mso-bookmark:_Hlk217561797'><span
                                                            style='font-size:12.0pt;line-height:115%'>
                                                            <o:p></o:p>
                                                        </span></span></p>
                                            </td><span style='mso-bookmark:_Hlk217561797'></span>
                                        </tr
                                    </table><span style='mso-bookmark:_Hlk217561797'></span>
                                </td><span style='mso-bookmark:_Hlk217561797'></span>
                            </tr>
                        </table>
                    </div><span style='mso-bookmark:_Hlk217561797'></span>
                </td><span style='mso-bookmark:_Hlk217561797'></span>
            </tr>
            <!-- FOOTER AREA -->
        </table>
    </div><span style='mso-bookmark:_Hlk217561797'></span>
    <p class=MsoNormal><span lang=TR>
            <o:p>&nbsp;</o:p>
        </span></p>
</div>");

            return sb.ToString();
        }

        private static string AWNotice(string originalBody)
        {
            var settings = Settings.Default;

            string topBanner = Utils.ImageHelper.ImageToBase64(Properties.Resources.topBannerAWNotice);
            string bottomBanner = Utils.ImageHelper.ImageToBase64(Properties.Resources.bottomBannerAWNotice);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"<div class=WordSection1>
    <div align=center>
        <table class=MsoNormalTable border=0 cellspacing=3 cellpadding=0 width=1018
            style='width:763.2pt;mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'>
            <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>
                <td width=""99%"" valign=top style='width:99.6%;background:#000734;padding:0cm 0cm 0cm 0cm'>
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=3 cellpadding=0 width=1000
                            style='width:750.0pt;mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'>
                            <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;mso-yfti-lastrow:yes'>
                                <td style='background:white;padding:0cm 0cm 0cm 0cm'>
                                    <table class=MsoNormalTable border=0 cellspacing=3 cellpadding=0 width=1141
                                        style='width:855.7pt;mso-cellspacing:2.2pt;mso-yfti-tbllook:1184;mso-padding-alt:0cm 0cm 0cm 0cm'>
                                        <tr style='mso-yfti-irow:0;mso-yfti-firstrow:yes;height:125.4pt'>
                                            <td valign=top
                                                style='padding:0cm 0cm 0cm 0cm;height:125.4pt'>
                                                <p class=MsoNormal
                                                    style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'><span
                                                        style='mso-fareast-language:EN-US;mso-no-proof:yes'><img
                                                            width=1134 height=170 id=""_x0000_i1026""
                                                            src=""{topBanner}""
                                                            alt=""topBanner""></span><span
                                                        style='mso-fareast-language:EN-US'>
                                                        <o:p></o:p>
                                                    </span></p>
                                            </td>
                                        </tr>
                                        <tr style='mso-yfti-irow:1;height:60.00pt'>
                                            <td align='{settings.HorizontalAlign}' valign='{settings.VerticalAlign}' style='padding:0cm 0cm 0cm 0cm;height:60.00pt'>
                                                {originalBody}
                                            </td>
                                        </tr>
                                        <tr style='mso-yfti-irow:2;mso-yfti-lastrow:yes;height:96.35pt'>
                                            <td style=';padding:0cm 0cm 0cm 0cm;height:96.35pt'>
                                                <p class=MsoNormal
                                                    style='mso-margin-top-alt:auto;mso-margin-bottom-alt:auto'><span
                                                        style='mso-fareast-language:EN-US;mso-no-proof:yes'><img
                                                            width=1134 height=130 id=""_x0000_i1025""
                                                            src=""{bottomBanner}""
                                                            alt=""bottomBanner""></span><span
                                                        style='mso-fareast-language:EN-US'>
                                                        <o:p></o:p>
                                                    </span></p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <!-- FOOTER AREA -->
        </table>
    </div>
</div>");

            return sb.ToString();
        }

        private static string TemplateBasic(string originalBody, Image topBannerImage, Image bottomBannerImage)
        {
            var settings = Settings.Default;

            string topBanner = Utils.ImageHelper.ImageToBase64(topBannerImage);
            string bottomBanner = Utils.ImageHelper.ImageToBase64(bottomBannerImage);

            StringBuilder sb = new StringBuilder();

            sb.Append($@"<div class=WordSection1>
    <p class=MsoNormal>
        <o:p>&nbsp;</o:p>
    </p>
    <div align=center>
        <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1002 style=""width:751.6pt"">
            <tr>
                <td width=""100%"" valign=top style=""width:100.0%;background:#000734;padding:0cm 0cm 0cm 0cm"">
                    <div align=center>
                        <table class=MsoNormalTable border=0 cellspacing=1 cellpadding=0 width=1000
                            style=""width:750.0pt"">
                            <tr>
                                <td style=""background:white;padding:0cm 0cm 0cm 0cm"">
                                    <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0 width=1000
                                        style=""width:750.0pt"">
                                        <tr>
                                            <td valign=top style=""background:#000734;padding:0cm 0cm 0cm 0cm"">
                                                <p class=MsoNormal align=center
                                                    style=""text-align:center;line-height:115%""><span
                                                        style='font-size:12.0pt;line-height:115%;font-family:""Arial"",sans-serif'><img
                                                            width=1000 height=150
                                                            style='width:10.4166in;height:1.5625in' id=""_x0000_i1025""
                                                            src='{topBanner}'
                                                            alt=""topBanner"" /></span>
                                                    <o:p></o:p>
                                                </p>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""padding:0cm 0cm 0cm 0cm"">
                                                <table class=MsoNormalTable border=0 cellspacing=0 cellpadding=0
                                                    width=900 style=""width:700.00pt;margin:20.00pt"">
                                                    <tr style='mso-yfti-irow:1;height:60.00pt'>
                                                        <td align='{settings.HorizontalAlign}' valign='{settings.VerticalAlign}' style=""padding:0.3cm 0cm 0.3cm 0cm;height:60.00pt"">
                                                            {originalBody}
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style=""background:#000734;padding:0cm 0cm 0cm 0cm"">
                                                <p class=MsoNormal align=center
                                                    style=""text-align:center;line-height:115%""><span
                                                        style='font-size:12.0pt;line-height:115%;font-family:""Arial"",sans-serif'><img
                                                            border=0 width=1000 height=150
                                                            style='width:10.4166in;height:1.5625in' id=""_x0000_i1026""
                                                            src='{bottomBanner}'
                                                            alt=""bottomBanner"" /></span>
                                                    <o:p></o:p>
                                                </p>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
            <!-- FOOTER AREA -->
        </table>
    </div>
</div>");

            return sb.ToString();
        }


        /// <summary>
        /// Rengi koyulaştırır (gradient için)
        /// </summary>
        private static string AddFooter(string body)
        {
            var settings = Settings.Default;

            string footer = $@"
            <tr>
              <td
                style='padding: 4px; font-family:{settings.FooterFontFamily}; font-size:{settings.FooterFontSize}pt; line-height:16px; color:{settings.FooterColor};'>
                  {settings.FooterText}
              </td>
            </tr>";

            body = body.Replace("<!-- FOOTER AREA -->", footer);

            return body;
        }

        /// <summary>
        /// Rengi koyulaştırır (gradient için)
        /// </summary>
        private static string AdjustColor(string hexColor)
        {
            try
            {
                Color color = ColorTranslator.FromHtml(hexColor);
                int r = Math.Max(0, color.R - 40);
                int g = Math.Max(0, color.G - 40);
                int b = Math.Max(0, color.B - 40);
                return $"#{r:X2}{g:X2}{b:X2}";
            }
            catch
            {
                return "#764ba2";
            }
        }

        /// <summary>
        /// HTML özel karakterlerini escape eder
        /// </summary>
        private static string EscapeHtml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("&", "&amp;")
                .Replace("<", "&lt;")
                .Replace(">", "&gt;")
                .Replace("\"", "&quot;")
                .Replace("'", "&#39;")
                .Replace("\n", "<br/>");
        }
    }
}
