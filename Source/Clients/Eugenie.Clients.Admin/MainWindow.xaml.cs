namespace Eugenie.Clients.Admin
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Forms;
    using System.Windows.Input;

    using Common.Notifications;

    public partial class MainWindow
    {
        //private readonly NotifyIcon icon;

        public MainWindow()
        {
            this.InitializeComponent();
            this.txtSearch.Focus();
            //this.icon = new NotifyIcon();
            //this.icon.Icon = Properties.Resources.Icon;
            //this.icon.MouseClick +=
            //    delegate
            //    {
            //        this.Show();
            //        this.icon.Visible = false;
            //        this.WindowState = WindowState.Normal;
            //    };
        }

        //protected override void OnStateChanged(EventArgs e)
        //{
        //    if (this.WindowState == WindowState.Minimized)
        //    {
        //        this.icon.Visible = true;
        //        this.Hide();
        //    }
        //
        //    base.OnStateChanged(e);
        //}

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