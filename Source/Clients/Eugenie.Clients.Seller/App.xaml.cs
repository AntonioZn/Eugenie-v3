namespace Eugenie.Clients.Seller
{
    using System.IO;
    using System.Windows.Threading;

    using Common.Helpers;

    using Properties;

    public partial class App
    {
        public App()
        {
#if !DEBUG
            this.DispatcherUnhandledException += this.OnUnhandledException;
#endif
            this.InitializeDefaultPaths();
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogToFile(e.Exception, Settings.Default.LogPath);
            e.Handled = true;
        }

        private void InitializeDefaultPaths()
        {
            if (string.IsNullOrEmpty(Settings.Default.LogPath))
            {
                Settings.Default.LogPath = "C:\\EugenieLogs";
            }

            if (string.IsNullOrEmpty(Settings.Default.ReceiptPath))
            {
                Settings.Default.ReceiptPath = "C:\\EugenieReceipts";
            }

            Settings.Default.Save();

            if (!Directory.Exists(Settings.Default.LogPath))
            {
                Directory.CreateDirectory(Settings.Default.LogPath);
            }

            if (!Directory.Exists(Settings.Default.ReceiptPath))
            {
                Directory.CreateDirectory(Settings.Default.ReceiptPath);
            }

        }
    }
}
