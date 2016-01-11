namespace Eugenie.Clients.AdminPanel.Views
{
    using System.Windows;
    using System.Windows.Controls;

    public partial class Settings
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        private void PasswordTextBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic) this.DataContext).NewServer.Password = ((PasswordBox) sender).Password;
            }
        }

        private void AddButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.passwordBox.Password = null;
            ((dynamic)this.DataContext).HandleAddNewServerCommand(null);
        }
    }
}