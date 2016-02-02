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
            var dialogHost = this.AssociatedObject.FindName("dialogHost") as DialogHost;

            if (!dialogHost.IsOpen && (e.Key == Key.Enter || e.Key == Key.Delete || e.Key == Key.Escape || e.Key == Key.F1))
            {
                var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
                var userControl = contentControl.Content as UserControl;
                var dataContext = userControl?.DataContext;

                if (dataContext != null)
                {
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
                }
            }
        }
    }
}