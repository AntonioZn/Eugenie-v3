namespace Eugenie.Clients.Common.Handlers
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using MaterialDesignThemes.Wpf;

    public class NavigateDataGridWithArrowsHandler
    {
        private readonly Window window;

        public NavigateDataGridWithArrowsHandler(Window window)
        {
            this.window = window;
            window.PreviewKeyDown += this.OnPreviewKeyDown;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Down && e.Key != Key.Up) return;
            
            var dataGrid = this.GetDataGrid();
            if (dataGrid != null && dataGrid.Items.Count > 0)
            {
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
        }

        private DataGrid GetDataGrid()
        {
            var dialogHost = this.window.FindName("dialogHost") as DialogHost;
            if (dialogHost.IsOpen)
            {
                var dialogContent = dialogHost.DialogContent as UserControl;
                return dialogContent?.FindName("dataGrid") as DataGrid;
            }

            var contentControl = this.window.FindName("MainFrame") as ContentControl;
            var userControl = contentControl?.Content as UserControl;
            return userControl?.FindName("dataGrid") as DataGrid;
        }
    }
}