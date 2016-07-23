namespace Eugenie.Clients.Seller.Models
{
    using System;
    using System.IO;

    using Newtonsoft.Json;

    public class Settings
    {
        private readonly string settingsFilePath;

        public Settings()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var userFilePath = Path.Combine(localAppData, "Eugenie");
            this.settingsFilePath = Path.Combine(userFilePath, "SellerSettings.txt"); ;

            Directory.CreateDirectory(userFilePath);
        }

        public void Load()
        {
            if (File.Exists(this.settingsFilePath))
            {
                var settingsString = File.ReadAllText(this.settingsFilePath);
                var settings = JsonConvert.DeserializeObject<Settings>(settingsString);

                this.Address = settings.Address;
                this.LogPath = settings.LogPath;
                this.ReceiptPath = settings.ReceiptPath;
                this.IsSelfHost = settings.IsSelfHost;
                this.Port = settings.Port;
                this.BackupDatabase = settings.BackupDatabase;
                this.BackupHours = settings.BackupHours;
                this.BackupMinutes = settings.BackupMinutes;
                this.BackupPath = settings.BackupPath;
                this.LotteryUsername = settings.LotteryUsername;
                this.LotteryPassword = settings.LotteryPassword;
            }
        }

        public string Address { get; set; } = "";

        public string LogPath { get; set; } = "C:\\EugenieLogs";

        public string ReceiptPath { get; set; } = "C:\\EugenieReceipts";

        public bool IsSelfHost { get; set; }

        public int Port { get; set; } = 9000;

        public bool BackupDatabase { get; set; }

        public int BackupHours { get; set; } = 20;

        public int BackupMinutes { get; set; } = 0;

        public string BackupPath { get; set; } = "C:\\EugenieBackups";

        public string LotteryUsername { get; set; }

        public string LotteryPassword { get; set; }

        public void Save()
        {
            File.WriteAllText(this.settingsFilePath, JsonConvert.SerializeObject(this));
        }
    }
}
