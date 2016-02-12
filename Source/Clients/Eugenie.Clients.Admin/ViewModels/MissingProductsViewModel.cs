namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    public class MissingProductsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IServerManager manager;

        public MissingProductsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.MissingProducts = new ObservableCollection<MissingProduct>();

            this.Enter = new RelayCommand(this.HandleEnter);
        }

        public ICommand Enter { get; }

        public MissingProduct SelectedProduct { get; set; }

        public ObservableCollection<MissingProduct> MissingProducts { get; }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    this.HandleEnter();
                    e.Handled = true;
                    break;
            }
        }

        public void HandleEnter()
        {
            if (this.SelectedProduct == null)
            {
                return;
            }

            var viewModel = ViewModelLocator.container.Resolve<DeliveryViewModel>();
            viewModel.ImportMissingProduct(this.SelectedProduct.Name, this.SelectedProduct.Barcode);
            ViewModelLocator.container.Resolve<MainWindowViewModel>().ShowDelivery();
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