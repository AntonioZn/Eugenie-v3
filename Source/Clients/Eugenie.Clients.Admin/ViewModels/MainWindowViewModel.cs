﻿namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Input;

    using Common.Contracts;
    using Common.Еxtensions;

    using Domain;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Helpers;

    using Models;

    using Views;

    public class MainWindowViewModel : ViewModelBase, IKeyHandler, IBarcodeHandler
    {
        private readonly ServerManager manager;
        private Visibility loadingVisibility;
        private MenuItem selectedMenuItem;
        private ObservableCollection<ActiveServer> servers;

        public MainWindowViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.LoadingVisibility = Visibility.Visible;

            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.Refresh = new RelayCommand(this.HandleRefresh);
            this.Select = new RelayCommand<ActiveServer>(this.HandleSelect);

            // TODO: Дневник за входящ контрол
            this.Views = new List<MenuItem>
                         {
                             new MenuItem("Продукти", new ProductsEditor()),
                             new MenuItem("Доставка (F1)", new Delivery()),
                             new MenuItem("Отчети", new Reports()),
                             new MenuItem("Служители", new Sellers()),
                             new MenuItem("Липсващи продукти", new MissingProducts()),
                             new MenuItem("Продукти с изтичащ срок", new ExpiringProducts()),
                             new MenuItem("Продукти с ниска наличност", new LowQuantityProducts()),
                             new MenuItem("Национална лотария", new NationalLottery()),
                             new MenuItem("Настройки", new Settings())
                         };

            this.SelectedMenuItem = this.Views.First();
            this.manager.Initialize();
        }

        public ICommand Refresh { get; }

        public ICommand Select { get; }

        public Visibility LoadingVisibility
        {
            get => this.loadingVisibility;
            set
            {
                this.Set(() => this.LoadingVisibility, ref this.loadingVisibility, value);
            }
        }

        public ICollection<ActiveServer> Servers
        {
            get => this.servers ?? (this.servers = new ObservableCollection<ActiveServer>());
            set
            {
                this.servers = this.servers ?? new ObservableCollection<ActiveServer>();
                this.servers.Clear();
                value.ForEach(this.servers.Add);
            }
        }

        public IList<MenuItem> Views { get; }

        public MenuItem SelectedMenuItem
        {
            get => this.selectedMenuItem;
            set
            {
                this.Set(() => this.SelectedMenuItem, ref this.selectedMenuItem, value);
            }
        }

        public void ShowProductsEditor()
        {
            this.SelectedMenuItem = this.Views[0];
        }

        public void ShowDelivery()
        {
            this.SelectedMenuItem = this.Views[1];
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.F1:
                    this.ShowDelivery();
                    e.Handled = true;
                    break;
                case Key.F5:
                    this.HandleRefresh();
                    e.Handled = true;
                    break;
                default:
                    (this.SelectedMenuItem.Content.DataContext as IKeyHandler)?.HandleKey(e, key);
                    break;
            }
        }

        public void HandleBarcode(string barcode)
        {
            (this.SelectedMenuItem.Content.DataContext as IBarcodeHandler)?.HandleBarcode(barcode);
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
            this.Servers = this.manager.Stores.Where(x => x.Client != null).Select(x => new ActiveServer(x.Name)).ToList();
            this.LoadingVisibility = Visibility.Collapsed;
        }
    }
}