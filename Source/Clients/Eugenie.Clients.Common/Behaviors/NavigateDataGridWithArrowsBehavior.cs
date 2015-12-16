namespace Eugenie.Clients.Common.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;
    using System.Windows.Media;

    public class NavigateDataGridWithArrowsBehavior : Behavior<Window>
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
            if (e.Key == Key.Down || e.Key == Key.Up || e.Key == Key.Enter)
            {
                var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
                var contentPresenter = VisualTreeHelper.GetChild(contentControl, 0);
                var userControl = VisualTreeHelper.GetChild(contentPresenter, 0) as UserControl;
                var dataGrid = userControl.FindName("DataGrid") as DataGrid;
                var context = dataGrid.DataContext;

                if (dataGrid != null && dataGrid.Items.Count > 0)
                {
                    if (e.Key == Key.Enter)
                    {
                        //TODO: add window dialog
                    }
                    else if (e.Key == Key.Down && dataGrid.SelectedIndex < dataGrid.Items.Count)
                    {
                        dataGrid.SelectedIndex = dataGrid.SelectedIndex + 1;
                    }
                    else if (e.Key == Key.Up && dataGrid.SelectedIndex > 0)
                    {
                        dataGrid.SelectedIndex = dataGrid.SelectedIndex - 1;
                    }

                    if (dataGrid.SelectedItem != null)
                    {
                        dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    }
                }

                e.Handled = true;
            }
        }
    }
}