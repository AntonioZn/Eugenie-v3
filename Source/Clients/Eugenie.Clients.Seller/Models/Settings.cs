namespace Eugenie.Clients.Seller.Models
{
    public class Settings
    {
        public bool IsSelfHost { get; set; } = true;

        public int Port { get; set; } = 9000;

        public bool BackupDatabase { get; set; } = true;

        public string BackupPath { get; set; } = "C:\\EugenieBackups";

        public int BackupHours { get; set; } = 21;

        public int BackupMinutes { get; set; } = 0;

        public string ServerAddress { get; set; }

        public string ReceiptPath { get; set; } = "C:\\EugenieReceipts";

        public string LotteryUsername { get; set; }

        public string LotteryPassword { get; set; }

        public int OpenHours { get; set; } = 7;

        public int OpenMinutes { get; set; } = 30;

        public int CloseHours { get; set; } = 21;

        public int CloseMinutes { get; set; } = 0;

        public bool Shutdown { get; set; }

        public bool Sleep { get; set; }
    }
}