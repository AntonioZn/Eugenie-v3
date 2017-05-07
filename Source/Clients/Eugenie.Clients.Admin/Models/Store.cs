namespace Eugenie.Clients.Admin.Models
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using Newtonsoft.Json;

    public class Store : IValidatableObject
    {
        private readonly ObservableCollection<Product> products = new ObservableCollection<Product>();
        private readonly ObservableCollection<Report> reports = new ObservableCollection<Report>();
        private readonly ObservableCollection<MissingProduct> missingProducts = new ObservableCollection<MissingProduct>();
        private readonly ObservableCollection<Seller> sellers = new ObservableCollection<Seller>();

        public string Name { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        //TODO: Validate
        public string Address { get; set; }

        [JsonIgnore]
        public StoreClient Client { get; set; }

        [JsonIgnore]
        public ICollection<Product> Products
        {
            get => this.products;
            set
            {
                this.products.Clear();
                value.ForEach(x => this.products.Add(x));
            }
        }

        [JsonIgnore]
        public ICollection<Report> Reports
        {
            get => this.reports;
            set
            {
                this.reports.Clear();
                value.ForEach(x => this.reports.Add(x));
            }
        }

        [JsonIgnore]
        public ICollection<MissingProduct> MissingProducts
        {
            get => this.missingProducts;
            set
            {
                this.missingProducts.Clear();
                value.ForEach(x => this.missingProducts.Add(x));
            }
        }

        [JsonIgnore]
        public ICollection<Seller> Sellers
        {
            get => this.sellers;
            set
            {
                this.sellers.Clear();
                value.ForEach(x => this.sellers.Add(x));
            }
        }

        public string this[string propertyName]
        {
            get
            {
                switch (propertyName)
                {
                    case nameof(this.Name):
                        return Validator.ValidateServerName(this.Name);
                    case nameof(this.Username):
                        return Validator.ValidateUsername(this.Username);
                    case nameof(this.Password):
                        return Validator.ValidatePassword(this.Password);
                    default:
                        return null;
                }
            }
        }

        public string Error { get; }

        public bool HasNoValidationErrors()
        {
            return this[nameof(this.Name)] == null
                   && this[nameof(this.Username)] == null
                   && this[nameof(this.Password)] == null;
        }
    }
}