namespace Eugenie.Clients.Seller
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;

    using MaterialDesignThemes.Wpf;

    using Server.Host;

    using ViewModels;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.PreviewKeyDown += this.OnKeyDown;

#if DEBUG
            this.WindowStyle = WindowStyle.SingleBorderWindow;
            this.Topmost = false;
#endif
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            var keyHandler = this.GetHandler();

            keyHandler?.HandleKey(e, key);
        }

        private IKeyHandler GetHandler()
        {
            if (this.dialogHost.IsOpen)
            {
                var dialogContent = this.dialogHost.DialogContent as UserControl;
                return dialogContent?.DataContext as IKeyHandler;
            }

            return this.DataContext as IKeyHandler;
        }

        protected override void OnClosed(EventArgs e)
        {
            //ViewModelLocator.Container.Resolve<IWebApiHost>().Scheduler.Shutdown();
            base.OnClosed(e);
        }
    }
}