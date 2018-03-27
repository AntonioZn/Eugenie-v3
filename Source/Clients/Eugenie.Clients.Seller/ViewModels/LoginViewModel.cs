namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Autofac;

    using Common;
    using Common.Contracts;
    using Common.Exceptions;
    using Common.Helpers;

    using Helpers;

    using Sv.Wpf.Core.Controls;
    using Sv.Wpf.Core.Helpers;
    using Sv.Wpf.Core.Mvvm;
    using Sv.Wpf.Core.Mvvm.ValidationRules;

    public class LoginViewModel : ViewModelBase, IKeyHandler
    {
        private readonly INavigationService navigationService;
        private readonly TaskManager taskManager;

        public LoginViewModel(INavigationService navigationService, TaskManager taskManager)
        {
            this.navigationService = navigationService;
            this.taskManager = taskManager;
        }

        public ICommand LoginCommand => new RelayCommand(this.Login, () => this.taskManager.CanRun() && this.HasNoValidationErrors());

        [ValidateString(3, 20)]
        public string Username { get; set; }

        [ValidateString(3, 20)]
        public string Password { get; set; }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.F1:
                    this.navigationService.NavigateToAsync<SettingsViewModel>();
                    e.Handled = true;
                    break;
            }
        }

        private async void Login()
        {
            var task = new TaskManager.Task("Влизане", true);
            task.Function = async (cts, logger) =>
                            {
                                var client = new StoreClient(SettingsManager.Get().ServerAddress);

                                try
                                {
                                    await client.AuthenticateAsync(this.Username, this.Password, cts.Token);
                                    await this.navigationService.NavigateToAsync<SellViewModel>(client);
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