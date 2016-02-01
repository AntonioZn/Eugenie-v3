namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Helpers;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            this.Login = new RelayCommand<object>(this.HandleLogin);
        }

        public ICommand Login { get; set; }

        public string Username { get; set; }

        private void HandleLogin(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;
        }
    }
}
