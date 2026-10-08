using System;
using System.Configuration;
using System.IO;
internal sealed class MemorySettings : ApplicationSettingsBase
{
    public bool FailSave;
    public int SavedNumber;
    [UserScopedSetting, DefaultSettingValue("1")]
    public int Number { get { return (int)this[nameof(Number)]; } set { this[nameof(Number)] = value; } }
    public override void Save()
    {
        if (FailSave) throw new IOException("Storage unavailable");
        SavedNumber = Number;
    }
}
internal static class SettingsTests
{
    private static void Save(MemorySettings settings, Action apply)
    {
        MailFormatter.Utils.SettingsPersistence.Save(settings, apply);
    }
    public static void Run(Action<string, Action> check)
    {
        check("Failed settings save restores in-memory values", () => {
            var settings = new MemorySettings { Number = 7, FailSave = true };
            bool threw = false;
            try { Save(settings, () => settings.Number = 9); } catch(IOException) { threw = true; }
            if (!threw || settings.Number != 7) throw new Exception("Rollback failed");
        });
        check("Successful settings save persists changed values", () => {
            var settings = new MemorySettings { Number = 7 };
            Save(settings, () => settings.Number = 9);
            if (settings.Number != 9 || settings.SavedNumber != 9) throw new Exception("Save failed");
        });
        check("Validation error during settings update restores prior values", () => {
            var settings = new MemorySettings { Number = 7 };
            try { Save(settings, () => { settings.Number = 9; throw new ArgumentException(); }); } catch(ArgumentException) { }
            if (settings.Number != 7 || settings.SavedNumber != 0) throw new Exception("Partial update persisted");
        });
    }
}
