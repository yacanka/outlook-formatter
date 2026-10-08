using System;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using MailFormatter.Properties;
internal static class AssetTests
{
    public static void Run(Action<string, Action> check)
    {
        check("Every embedded image resource loads", () => {
            var resources = XDocument.Load("Properties/Resources.resx");
            foreach (var item in resources.Root.Elements("data"))
            {
                if (!((string)item.Attribute("type") ?? "").Contains("ResXFileRef")) continue;
                string name = (string)item.Attribute("name");
                var image = Resources.ResourceManager.GetObject(name) as Image;
                if (image == null || image.Width <= 0 || image.Height <= 0) throw new Exception("Image resource unavailable: " + name);
            }
        });
        check("Every project image exists and decodes", () => {
            XNamespace ns = "http://schemas.microsoft.com/developer/msbuild/2003";
            foreach (var item in XDocument.Load("Mailify.csproj").Descendants(ns + "None"))
            {
                string name = (string)item.Attribute("Include") ?? "";
                if (!name.StartsWith("Resources\\") || (!name.EndsWith(".png") && !name.EndsWith(".jpg"))) continue;
                string path = name.Replace('\\', Path.DirectorySeparatorChar);
                using (var image = Image.FromFile(path))
                {
                    if (image.Width <= 0 || image.Height <= 0) throw new Exception("Invalid image: " + name);
                }
            }
        });
    }
}
