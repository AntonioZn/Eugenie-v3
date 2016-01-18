namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.ObjectModel;

    using Common.Contracts;
    using Common.Models;

    using GalaSoft.MvvmLight;

    using Views;

    public class EarningsViewModel : ViewModelBase
    {
        private readonly IServerManager manager;

        public EarningsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.TabContents = new ObservableCollection<TabContent>();
        }

        public ObservableCollection<TabContent> TabContents { get; }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.TabContents.Clear();
            this.TabContents.Add(new TabContent("Горе", new Delivery()));
            this.TabContents.Add(new TabContent("Долу", new Settings()));
        }
    }
}
