namespace Eugenie.Clients.Seller
{
    using System.Windows.Threading;

    using Common.Helpers;

    using Properties;

    public partial class App
    {
        public App()
        {
            this.InitializeComponent();
            this.DispatcherUnhandledException += this.OnUnhandledException;
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Logger.LogToFile(e.Exception, Settings.Default.LogPath);
            e.Handled = true;
        }
    }
}