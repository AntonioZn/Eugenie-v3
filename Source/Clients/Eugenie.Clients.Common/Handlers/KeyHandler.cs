namespace Eugenie.Clients.Common.Handlers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Contracts;

    using MaterialDesignThemes.Wpf;

    public class KeyHandler
    {
        private readonly Window window;

        public KeyHandler(Window window)
        {
            this.window = window;
            window.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            var keyHandler = this.GetHandler();

            keyHandler?.HandleKey(e, key);
        }

        private IKeyHandler GetHandler()
        {
            var dialogHost = this.window.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.DataContext as IKeyHandler;
            }

            return this.window.DataContext as IKeyHandler;
        }
    }
}