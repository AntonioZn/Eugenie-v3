namespace Eugenie.Clients.Seller.Models
{
    public class Settings
    {
        public string Address { get; set; } = "";

        public string LogPath { get; set; } = "C:\\EugenieLogs";

        public string ReceiptPath { get; set; } = "C:\\EugenieReceipts";

        public bool IsSelfHost { get; set; }

        public int Port { get; set; } = 9000;

        public bool BackupDatabase { get; set; }

        public int BackupHours { get; set; } = 20;

        public int BackupMinutes { get; set; } = 0;

        public string BackupPath { get; set; } = "C:\\EugenieBackups";
    }
}
