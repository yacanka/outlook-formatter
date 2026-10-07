using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Outlook = Microsoft.Office.Interop.Outlook;


namespace MailFormatter.Utils
{
    public static class ImageHelper
    {
        /// <summary>
        /// Resimleri bytes dizisine çevirir
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] ToPngBytes(Image img)
        {
            using (var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Resmi Base64'e çevirir
        /// </summary>
        public static string ImageToBase64(Image image)
        {
            if (image == null) return "";

            ImageFormat format = image.RawFormat;

            string mimeType = GetMimeType(image);

            using (var ms = new MemoryStream())
            {
                ImageFormat saveFormat = GetSaveFormat(format);
                image.Save(ms, saveFormat);

                return $"data:{mimeType};base64,{Convert.ToBase64String(ms.ToArray())}";
            }
        }

        private static string GetMimeType(Image image)
        {
            var codec = ImageCodecInfo.GetImageEncoders().FirstOrDefault(c => c.FormatID == image.RawFormat.Guid);
            return codec?.MimeType ?? "image/png";
        }

        private static ImageFormat GetSaveFormat(ImageFormat format)
        {
            if (format.Guid == ImageFormat.MemoryBmp.Guid)
            {
                return ImageFormat.Png;
            }

            return format;
        }

        /// <summary>
        /// Varsayılan logo oluşturur
        /// </summary>
        public static string CreateDefaultLogo(string path)
        {
            try
            {
                using (Bitmap bmp = new Bitmap(200, 80))
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.Clear(Color.FromArgb(102, 126, 234));
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    using (Font font = new Font("Segoe UI", 18, FontStyle.Bold))
                    using (Brush brush = new SolidBrush(Color.White))
                    {
                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };
                        g.DrawString("LOGO", font, brush,
                            new RectangleF(0, 0, 200, 80), sf);
                    }

                    bmp.Save(path, ImageFormat.Png);
                }
                return path;
            }
            catch
            {
                return null;
            }
        }
    }
}
