namespace Eugenie.Clients.Seller.Views
{
    using System.Timers;
    using System.Windows;

    using MaterialDesignThemes.Wpf;

    public partial class MissingProduct
    {
        private readonly Timer timer;

        public MissingProduct()
        {
            this.InitializeComponent();
            this.timer = new Timer(3000);
            this.timer.Elapsed += this.OnElapsed;
            this.timer.Start();
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            this.timer.Stop();
            Application.Current.Dispatcher.Invoke(() =>
                                                  {
                                                      DialogHost.CloseDialogCommand.Execute(false, null);
                                                  });
        }
    }
}
