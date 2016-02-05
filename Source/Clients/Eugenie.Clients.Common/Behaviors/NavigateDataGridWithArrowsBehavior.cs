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

            if (e.Key == Key.Down)
            {
                if (dataGrid.SelectedIndex < dataGrid.Items.Count - 1)
                {
                    dataGrid.SelectedIndex += 1;
                }
                else
                {
                    dataGrid.SelectedIndex = 0;
                }

                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (dataGrid.SelectedIndex > 0)
                {
                    dataGrid.SelectedIndex -= 1;
                }
                else
                {
                    dataGrid.SelectedIndex = dataGrid.Items.Count - 1;
                }

                dataGrid.ScrollIntoView(dataGrid.SelectedItem);
                e.Handled = true;
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