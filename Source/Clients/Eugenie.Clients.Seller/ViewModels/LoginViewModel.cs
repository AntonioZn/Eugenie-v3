﻿namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.Notifications;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Views;

    public class LoginViewModel : ViewModelBase, IKeyHandler
    {
        public LoginViewModel()
        {
            this.Login = new RelayCommand<object>(this.HandleLogin);
        }

        public ICommand Login { get; set; }

        public string Username { get; set; }

        private async void HandleLogin(object obj)
        {
            var passwordBox = obj as PasswordBox;
            var password = passwordBox.Password;

            var server = new ServerInformation
            {
                Addresses = new List<string> { Properties.Settings.Default.Address },
                Password = password,
                Username = this.Username
            };

            var client = await ServerTester.TestServerAsync(server);
            if (client == null)
            {
                NotificationsHost.Error("Грешно име или парола", "Въведените парола или име са невалидни.");
            }
            else
            {
                ViewModelLocator.httpClient = client;
                ViewModelLocator.container.Resolve<MainWindowViewModel>().Content = new Sell();
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.F1:
                    ViewModelLocator.container.Resolve<MainWindowViewModel>().Content = new Settings();
                    e.Handled = true;
                    break;
            }
        }
    }
}
