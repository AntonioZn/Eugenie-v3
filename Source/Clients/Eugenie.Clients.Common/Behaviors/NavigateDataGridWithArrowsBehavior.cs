namespace Eugenie.Clients.Common.Behaviors
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Interactivity;

    using MaterialDesignThemes.Wpf;

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
            var dataGrid = this.GetDataGrid();

            if (e.Key == Key.Down || e.Key == Key.Up)
            {
                if (dataGrid?.Items.Count > 0)
                {
                    if (e.Key == Key.Down && dataGrid.SelectedIndex < dataGrid.Items.Count)
                    {
                        dataGrid.SelectedIndex += 1;
                    }
                    else if (e.Key == Key.Up && dataGrid.SelectedIndex > 0)
                    {
                        dataGrid.SelectedIndex -= 1;
                    }

                    if (dataGrid.SelectedItem != null)
                    {
                        dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                    }

                    e.Handled = true;
                }
            }
        }

        private DataGrid GetDataGrid()
        {
            var dialogHost = this.AssociatedObject.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.FindName("dataGrid") as DataGrid;
            }

            var contentControl = this.AssociatedObject.FindName("MainFrame") as ContentControl;
            var userControl = contentControl?.Content as UserControl;
            return userControl?.FindName("dataGrid") as DataGrid;
        }
    }
}