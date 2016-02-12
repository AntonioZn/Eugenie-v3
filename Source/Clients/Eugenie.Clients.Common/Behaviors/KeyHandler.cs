namespace Eugenie.Clients.Common.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Contracts;

    using MaterialDesignThemes.Wpf;

    public class KeyHandler : Behavior<Window>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PreviewKeyDown += this.PreviewKeyDown;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PreviewKeyDown -= this.PreviewKeyDown;
        }

        private void PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var key = e.Key == Key.System ? e.SystemKey : e.Key;
            var keyHandler = this.GetHandler();

            keyHandler?.HandleKey(e, key);
        }

        private IKeyHandler GetHandler()
        {
            var dialogHost = this.AssociatedObject.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.DataContext as IKeyHandler;
            }

            return this.AssociatedObject.DataContext as IKeyHandler;
        }
    }
}