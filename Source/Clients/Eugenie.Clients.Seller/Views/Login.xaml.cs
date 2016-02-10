namespace Eugenie.Clients.Seller.Views
{
    using System.Windows;

    public partial class Login
    {
        public Login()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.txtName.Focus();
        }
    }
}