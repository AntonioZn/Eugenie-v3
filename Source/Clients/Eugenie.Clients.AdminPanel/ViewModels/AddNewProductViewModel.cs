namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;

    using Data.Models;

    using GalaSoft.MvvmLight;

    public class AddNewProductViewModel : ViewModelBase
    {
        private readonly IServerManager manager;
        private ObservableCollection<string> exisitingProducts; 

        public AddNewProductViewModel(IServerManager manager)
        {
            this.manager = manager;
            manager.ServerTestingFinished += this.OnServerTestingFinished;

            this.Measures = new List<MeasureType>();
            foreach (var measureType in Enum.GetValues(typeof(MeasureType)))
            {
                this.Measures.Add((MeasureType)measureType);
            }
        }

        private async void OnServerTestingFinished(object sender, EventArgs e)
        {
            var products = await this.manager.GetProductsByPageAsync(1, 2000);
            this.ExistingProductsNames = products.Select(x => x.Name);
        }

        public IEnumerable<string> ExistingProductsNames
        {
            get
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<string>();
                }

                return this.exisitingProducts;
            }

            set
            {
                if (this.exisitingProducts == null)
                {
                    this.exisitingProducts = new ObservableCollection<string>();
                }

                this.exisitingProducts.Clear();
                foreach (var simplifiedProduct in value)
                {
                    this.exisitingProducts.Add(simplifiedProduct);
                }
            }
        }

        public ICollection<MeasureType> Measures { get; set; }
    }
}
