namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.IO;

    using Models;

    using Newtonsoft.Json;

    public class SettingsManager
    {
        private readonly string settingsFile;

        public static SettingsManager Default { get; } = new SettingsManager();

        public SettingsManager()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var userFilePath = Path.Combine(localAppData, "Eugenie");
            this.settingsFile = Path.Combine(userFilePath, "SellerSettings.txt");;

            Directory.CreateDirectory(userFilePath);

            if (File.Exists(this.settingsFile))
            {
                var settingsString = File.ReadAllText(this.settingsFile);
                this.Settings = JsonConvert.DeserializeObject<Settings>(settingsString);
            }
        }

        public Settings Settings { get; } = new Settings();

        public void Save()
        {
            File.WriteAllText(this.settingsFile, JsonConvert.SerializeObject(this.Settings));
        }
    }
}
