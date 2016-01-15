namespace Eugenie.Clients.AdminPanel.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    using Common.Contracts;

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

            if (!dialogHost.IsOpen && (e.Key == Key.Enter || e.Key == Key.Delete))
            {
                var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
                var contentPresenter = VisualTreeHelper.GetChild(contentControl, 0);
                var userControl = VisualTreeHelper.GetChild(contentPresenter, 0) as UserControl;
                var dataContext = userControl?.DataContext;

                if (dataContext != null)
                {
                    if (e.Key == Key.Enter && dataContext is IEnterHandler)
                    {
                        ((IEnterHandler)dataContext).HandleEnter();
                        e.Handled = true;
                    }

                    if (e.Key == Key.Delete && dataContext is IDeleteHandler)
                    {
                        ((IDeleteHandler)dataContext).HandleDelete();
                        e.Handled = true;
                    }
                }
            }
        }
    }
}