namespace Eugenie.Clients.AdminPanel
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    public partial class MainWindow
    {
        public MainWindow()
        {
            this.InitializeComponent();
            this.txtSearch.Focus();
        }

        private void UIElement_OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.MenuToggleButton.IsChecked = false;
        }

        private void FlyoutMenuListBox_OnGotFocus(object sender, RoutedEventArgs e)
        {
            this.txtSearch.Focus();
        }

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.flyoutMenuListBox.SelectedIndex != 0)
            {
                this.flyoutMenuListBox.SelectedIndex = 0;
            }
        }
    }
}