﻿namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Exceptions;
    using Common.Helpers;

    using Models;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Helpers;
    using Sv.Wpf.Core.Mvvm;
    using Sv.Wpf.Core.Mvvm.ValidationRules;

    public class LoginViewModel : ViewModelBase, IKeyHandler
    {
        private readonly TaskManager taskManager;
        private readonly Settings settings;

        public LoginViewModel(TaskManager taskManager, Settings settings)
        {
            this.taskManager = taskManager;
            this.settings = settings;
        }

        public ICommand LoginCommand => new RelayCommand<PasswordBox>(this.HandleLogin, (pb) => this.taskManager.CanRun() && this.HasNoValidationErrors() && pb.Password.Length >= 4 && pb.Password.Length < 20);

        [ValidateString(3, 20)]
        public string Username { get; set; }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.F1:
                    ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowSettings();
                    e.Handled = true;
                    break;
            }
        }

        private async void HandleLogin(PasswordBox passwordBox)
        {
            var task = new TaskManager.Task("Влизане", true);
            task.Function = async (cts, logger) =>
                            {
                                var client = new StoreClient(this.settings.Address);

                                try
                                {
                                    await client.AuthenticateAsync(this.Username, passwordBox.Password, cts.Token);
                                    ViewModelLocator.Container.Resolve<MainWindowViewModel>().Client = client;
                                    ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowSell();
                                    return;
                                }
                                catch (LoginException)
                                {
                                    NotificationsHost.Error("Notifications", "Грешно име или парола", "Въведените име или парола са невалидни.");
                                }
                                catch (HttpRequestException)
                                {
                                    NotificationsHost.Error("Notifications", "Грешка", "Няма връзка към сървъра.");
                                }
                                catch (TaskCanceledException)
                                {
                                    client.Dispose();
                                    throw;
                                }

                                client.Dispose();
                            };

            await this.taskManager.Run(task);
        }
    }
}