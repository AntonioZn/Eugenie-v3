namespace Eugenie.Clients.Common.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using Common.Contracts.KeyHandlers;

    using MaterialDesignThemes.Wpf;

    public class KeyHandlersBehavior : Behavior<Window>
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
            var dataContext = this.GetDataContext();

            if (e.Key == Key.Enter && dataContext is IEnterHandler)
            {
                ((IEnterHandler)dataContext).HandleEnter();
                e.Handled = true;
            }
            else if (e.Key == Key.Delete && dataContext is IDeleteHandler)
            {
                ((IDeleteHandler)dataContext).HandleDelete();
                e.Handled = true;
            }
            else if (e.Key == Key.Escape && dataContext is IEscapeHandler)
            {
                ((IEscapeHandler)dataContext).HandleEscape();
                e.Handled = true;
            }
            else if (e.Key == Key.F1 && dataContext is IF1Handler)
            {
                ((IF1Handler)dataContext).HandleF1();
                e.Handled = true;
            }
            else if (e.SystemKey == Key.F10 && dataContext is IF10Handler)
            {
                ((IF10Handler)dataContext).HandleF10();
                e.Handled = true;
            }
            else if (e.Key == Key.F11 && dataContext is IF11Handler)
            {
                ((IF11Handler)dataContext).HandleF11();
                e.Handled = true;
            }
            else if (e.Key == Key.F12 && dataContext is IF12Handler)
            {
                ((IF12Handler)dataContext).HandleF12();
                e.Handled = true;
            }
            else if (e.Key == Key.System && e.SystemKey == Key.F5 && dataContext is IAltF5Handler)
            {
                ((IAltF5Handler)dataContext).HandleAltF5();
                e.Handled = true;
            }
        }

        private object GetDataContext()
        {
            var dialogHost = this.AssociatedObject.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.DataContext;
            }

            var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
            var userControl = contentControl?.Content as UserControl;
            return userControl?.DataContext;
        }
    }
}