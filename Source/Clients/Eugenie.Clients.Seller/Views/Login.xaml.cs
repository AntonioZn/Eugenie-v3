namespace Eugenie.Clients.Seller.Views
{
    using System.Windows;

    using ViewModels;

    public partial class Login
    {
        private readonly LoginViewModel viewModel;

        public Login()
        {
            this.InitializeComponent();
            this.Loaded += this.OnLoaded;
            this.viewModel = (LoginViewModel) this.DataContext;
            this.txtPassword.PasswordChanged += (sender, args) => this.viewModel.Password = this.txtPassword.Password;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.txtName.Focus();
        }
    }
}