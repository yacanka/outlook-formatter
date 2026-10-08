using System;
using System.Collections.Generic;
using System.Configuration;

namespace MailFormatter.Utils
{
    internal static class SettingsPersistence
    {
        /// <summary>
        /// Applies and saves scalar user settings. Restores the prior in-memory values on
        /// failure, then rethrows so the form can remain open for correction or retry.
        /// </summary>
        public static void Save(ApplicationSettingsBase settings, Action applyChanges)
        {
            if (settings == null) throw new ArgumentNullException(nameof(settings));
            if (applyChanges == null) throw new ArgumentNullException(nameof(applyChanges));
            var previous = new Dictionary<string, object>();
            foreach (SettingsProperty property in settings.Properties)
            {
                if (!property.IsReadOnly) previous.Add(property.Name, settings[property.Name]);
            }
            try
            {
                applyChanges();
                settings.Save();
            }
            catch
            {
                foreach (var value in previous) settings[value.Key] = value.Value;
                throw;
            }
        }
    }
}
