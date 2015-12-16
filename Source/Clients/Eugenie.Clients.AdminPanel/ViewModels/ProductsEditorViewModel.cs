namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Common;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class ProductsEditorViewModel : ViewModelBase
    {
        private readonly WebApiServerClient client;
        private ObservableCollection<SimplifiedProduct> products;

        private SimplifiedProduct selectedItem;

        public ProductsEditorViewModel()
        {
            this.client = new WebApiServerClient(new Uri("http://localhost:9000"), "AQAAANCMnd8BFdERjHoAwE_Cl-sBAAAAghKFVRs7OE6YRBoflNwdbgAAAAACAAAAAAAQZgAAAAEAACAAAADcux-gBEcAR-f0csaFobLE8k_10RDytz9lvmB_3ynQ7gAAAAAOgAAAAAIAACAAAAATpHJNm48sAugHmhkK3ygZYbXCCQRAPbdtzviyi3aVwUABAABh6kZKpslmn1zFHhWSYnOzsQOS_k5PrKHTsEJMmmJuzIkUG52SDsyr8NZIdu3tE0fBZ9H9r9qGwNaO4HOZOEm0k47EBQ0XLl4YAgRFDkFmOEMqfQpU0HesVVnHtn0gvjpObEaH8FwrX_aZnTfTUf2sRjjgMzClq_m84ugAk_5WOq4rZoFT9QlpkGidl8R7gawy17t-ofb4mFoYWabKmomhsBVIbCXyzfoM4f_T2K0aUWplusogSC0GUiNwV4wY7U7yuXtgDH3cdlytL03nDGA3H8lQ5JAu5mbWrnW7ftxrT7DBogCcVb9L4AS4QLalcfC0vsHRpmOta0eNVtO02yYnAZRsl7jGiM2hYggJtPxdvr79ex-4GewXybw9qLJs_ngwX6KsNN3_YkTLNAJxYmcHaltHvS2Fr7n36VdFmStT6EAAAADH9Ntz_aHlCL1cEbrUN-AEhO-PcVhlDeM99J0xNuBLqlv4Vi5bohrOabxd85Osde4wrv1iNVM7RRyMe1n05W9B");
            this.Initialize();
        }

        public IEnumerable<SimplifiedProduct> Products
        {
            get
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                return this.products;
            }

            set
            {
                if (this.products == null)
                {
                    this.products = new ObservableCollection<SimplifiedProduct>();
                }

                this.products.Clear();
                foreach (var product in value)
                {
                    this.products.Add(product);
                }
            }
        }

        public SimplifiedProduct SelectedItem
        {
            get
            {
                return this.selectedItem;
            }
            set
            {
                Set(() => this.SelectedItem, ref this.selectedItem, value);
            }
        }

        private async void Initialize()
        {
            this.Products = await this.client.GetProductsByPageAsync(1, 2000);
        }
    }
}
