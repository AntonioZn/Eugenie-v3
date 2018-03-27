namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.IO;

    using Models;

    using Newtonsoft.Json;

    public static class SettingsManager
    {
        private static readonly string settingsFilePath;

        static SettingsManager()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var userFilePath = Path.Combine(localAppData, "Eugenie");
            settingsFilePath = Path.Combine(userFilePath, "SellerSettings.txt"); ;

            Directory.CreateDirectory(userFilePath);
        }

        public static Settings Get()
        {
            if (File.Exists(settingsFilePath))
            {
                var settingsString = File.ReadAllText(settingsFilePath);
                var settings = JsonConvert.DeserializeObject<Settings>(settingsString);

                return settings;
            }

            return new Settings();
        }

        public static void Save(Settings settings)
        {
            File.WriteAllText(settingsFilePath, JsonConvert.SerializeObject(settings));
        }
    }
}
