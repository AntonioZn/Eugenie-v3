namespace Eugenie.Clients.Seller
{
    using System;
    using System.Windows;

    using Common.Behaviors;

    using ViewModels;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            new BarcodeScannerHandler(this);
            new KeyHandler(this);
            new NavigateDataGridWithArrowsHandler(this);
            this.notif.Loaded += (sender, args) => ((MainWindowViewModel) this.DataContext).InitializeAsync(null);
            
#if DEBUG
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.Topmost = false;
#endif
        }

        protected override void OnClosed(EventArgs e)
        {
            //ViewModelLocator.Container.Resolve<IWebApiHost>().Scheduler.Shutdown();
            base.OnClosed(e);
        }
    }
}