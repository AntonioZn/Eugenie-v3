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

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Helpers;

    public class MissingProductsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly ServerManager manager;

        public MissingProductsViewModel(ServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
            
            this.Enter = new RelayCommand(this.HandleEnter);
        }

        public ICommand Enter { get; }

        public MissingProduct SelectedProduct { get; set; }

        public ObservableCollection<MissingProduct> MissingProducts { get; } = new ObservableCollection<MissingProduct>();

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

            var viewModel = ViewModelLocator.Container.Resolve<DeliveryViewModel>();
            viewModel.ImportMissingProduct(this.SelectedProduct.Name, this.SelectedProduct.Barcode);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().ShowDelivery();
        }

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            var hashset = new HashSet<MissingProduct>();
            this.manager.Stores.ForEach(x => hashset.UnionWith(x.MissingProducts));

            this.MissingProducts.Clear();
            hashset.ForEach(this.MissingProducts.Add);
        }
    }
}