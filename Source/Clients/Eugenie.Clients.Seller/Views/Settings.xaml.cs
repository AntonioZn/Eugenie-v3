namespace Eugenie.Clients.Seller.Views
{
    using ViewModels;

    public partial class Settings
    {
        private readonly SettingsViewModel viewModel;

        public Settings()
        {
            this.InitializeComponent();
            this.viewModel = (SettingsViewModel) this.DataContext;
            this.passwordBox.Password = this.viewModel.LotteryPassword;
            this.passwordBox.PasswordChanged += (sender, args) => this.viewModel.LotteryPassword = this.passwordBox.Password;
        }
    }
}