namespace Eugenie.Clients.Seller.Views
{
    using System.Windows;

    using Autofac;

    using ViewModels;

    public partial class Settings
    {
        private readonly Models.Settings settings;

        public Settings()
        {
            this.InitializeComponent();
            this.passwordBox.PasswordChanged += this.OnPasswordChanged;
            this.settings = ViewModelLocator.Container.Resolve<Models.Settings>();
            this.passwordBox.Password = this.settings.LotteryPassword;
        }

        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            this.settings.LotteryPassword = this.passwordBox.Password;
        }
    }
}