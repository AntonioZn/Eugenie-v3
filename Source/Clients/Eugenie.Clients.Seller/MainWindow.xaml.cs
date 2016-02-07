namespace Eugenie.Clients.Seller
{
    using System;
    using System.Windows;

    using Common.Notifications;

    using Server.Host;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            #if DEBUG
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.Topmost = false;
            }
#endif
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationsHost.SetOwner(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            NotificationsHost.Close();
            Host.scheduler.Shutdown();
            base.OnClosed(e);
        }
    }
}