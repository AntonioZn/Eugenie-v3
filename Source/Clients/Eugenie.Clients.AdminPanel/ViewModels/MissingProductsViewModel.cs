﻿namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.Models;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class MissingProductsViewModel : ViewModelBase, IEnterHandler
    {
        private readonly IServerManager manager;

        public MissingProductsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.MissingProducts = new ObservableCollection<MissingProduct>();

            this.HandleEnterCommand = new RelayCommand(this.HandleEnter);
        }

        public ICommand HandleEnterCommand { get; }

        public MissingProduct SelectedProduct { get; set; }

        public ObservableCollection<MissingProduct> MissingProducts { get; }
        
        public async void HandleEnter()
        {
            if (this.SelectedProduct == null)
            {
                return;
            }

            var viewModel = ViewModelLocator.container.Resolve<DeliveryViewModel>();
            viewModel.Name = this.SelectedProduct.Name;
            viewModel.MainProductViewModel.Product.Barcodes.Add(new Barcode(this.SelectedProduct.Barcode));
            var dialog = new Delivery(true);
            await DialogHost.Show(dialog, "RootDialog");
        }


        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            var hashset = new HashSet<MissingProduct>();
            this.manager.Cache.MissingProductsPerServer.Values.ForEach(hashset.UnionWith);

            this.MissingProducts.Clear();
            hashset.ForEach(this.MissingProducts.Add);
        }
    }
}