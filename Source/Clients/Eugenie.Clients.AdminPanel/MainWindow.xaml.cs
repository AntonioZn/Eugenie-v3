namespace Eugenie.Clients.AdminPanel
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Notifications;

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

        private void TxtSearch_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.flyoutMenuListBox.SelectedIndex != 0)
            {
                this.flyoutMenuListBox.SelectedIndex = 0;
            }
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            NotificationsHost.SetOwner(this);
        }

        protected override void OnClosed(EventArgs e)
        {
            NotificationsHost.Close();
            base.OnClosed(e);
        }
    }
}