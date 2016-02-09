namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Еxtensions;

    using Contracts;

    using Domain;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Models;

    using Views;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IServerManager manager;
        private Visibility loadingVisibility;
        private MenuItem selectedMenuItem;
        private ObservableCollection<ActiveServer> servers;

        public MainWindowViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.LoadingVisibility = Visibility.Visible;

            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.Refresh = new RelayCommand(this.HandleRefresh);
            this.Select = new RelayCommand<ActiveServer>(this.HandleSelect);

            this.Views = new List<MenuItem>
                         {
                             new MenuItem("Продукти", new ProductsEditor()),
                             new MenuItem("Доставка", new Delivery()),
                             new MenuItem("Отчети", new Reports()),
                             new MenuItem("Служители", new Sellers()),
                             new MenuItem("Липсващи продукти", new MissingProducts()),
                             new MenuItem("Продукти с изтичащ срок", new ExpiringProducts()),
                             new MenuItem("Продукти с ниска наличност", new LowQuantityProducts()),
                             new MenuItem("Национална лотария", new NationalLottery()),
                             new MenuItem("Настройки", new Settings())
                         };

            this.SelectedMenuItem = this.Views.First();
        }

        public ICommand Refresh { get; }

        public ICommand Select { get; }

        public Visibility LoadingVisibility
        {
            get
            {
                return this.loadingVisibility;
            }

            set
            {
                this.Set(() => this.LoadingVisibility, ref this.loadingVisibility, value);
            }
        }

        public ICollection<ActiveServer> Servers
        {
            get
            {
                return this.servers ?? (this.servers = new ObservableCollection<ActiveServer>());
            }
            set
            {
                this.servers = this.servers ?? new ObservableCollection<ActiveServer>();
                this.servers.Clear();
                value.ForEach(this.servers.Add);
            }
        }

        public IEnumerable<MenuItem> Views { get; }

        public MenuItem SelectedMenuItem
        {
            get
            {
                return this.selectedMenuItem;
            }

            set
            {
                this.Set(() => this.SelectedMenuItem, ref this.selectedMenuItem, value);
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            var keyHandler = this.SelectedMenuItem.Content.DataContext as IKeyHandler;
            if (keyHandler != null && keyHandler != this)
            {
                keyHandler.HandleKey(e, key);
            }
        }

        private void HandleRefresh()
        {
            this.LoadingVisibility = Visibility.Visible;
            this.manager.Initialize();
        }

        private void HandleSelect(ActiveServer selectedServer)
        {
            foreach (var server in this.Servers)
            {
                server.IsSelected = false;
            }

            selectedServer.IsSelected = true;

            this.manager.SetSelectedServer(selectedServer.Name);
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.Servers = this.manager.Cache.ProductsPerServer.Keys.Where(x => x.Client != null).Select(x => new ActiveServer(x.Name)).ToList();
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}