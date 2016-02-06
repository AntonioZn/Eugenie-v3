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
            this.InitializeComponent();
            this.DispatcherUnhandledException += this.OnUnhandledException;
            this.InitializeDefaultPaths();
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogToFile(e.Exception, Settings.Default.LogPath);
            e.Handled = true;
        }

        private void InitializeDefaultPaths()
        {
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
