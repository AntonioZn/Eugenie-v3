namespace Eugenie.Clients.Seller.Views
{
    using System;
    using System.Windows;
    using System.Windows.Threading;

    using MaterialDesignThemes.Wpf;

    public partial class MissingProduct
    {
        private readonly DispatcherTimer timer;

        public MissingProduct()
        {
            this.InitializeComponent();
            this.Unloaded += this.OnUnloaded;
            this.timer = new DispatcherTimer();
            this.timer.Interval = TimeSpan.FromSeconds(3);
            this.timer.Tick += this.OnElapsed;
            this.timer.Start();
        }

        private void OnElapsed(object sender, EventArgs e)
        {
            this.timer.Stop();
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.timer.Stop();
        }
    }
}