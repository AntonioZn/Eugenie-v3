namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using Common.Contracts;

    using GalaSoft.MvvmLight;

    public class EarningsViewModel : ViewModelBase
    {
        private readonly IServerManager manager;

        public EarningsViewModel(IServerManager manager)
        {
            this.manager = manager;
        }
    }
}
