namespace Eugenie.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Barcode> barcodesRepository;
        private readonly IReportsService reportsService;

        public ProductsService(IRepository<Product> productsRepository, IRepository<Barcode> barcodesRepository, IReportsService reportsService)
        {
            this.productsRepository = productsRepository;
            this.barcodesRepository = barcodesRepository;
            this.reportsService = reportsService;
        }

        public void Delete(string name)
        {
            var product = this.productsRepository.All().FirstOrDefault(x => x.Name == name);
            this.productsRepository.Delete(product);
            this.productsRepository.SaveChanges();
        }

        public int Count()
        {
            return this.productsRepository.All().Count();
        }

        public IQueryable<Product> All()
        {
            return this.productsRepository.All().Include("Barcodes").Include("ExpirationDates");
        }

        //TODO: if not product is found add it to missing products
        public IQueryable<Product> GetByBarcode(string barcode)
        {
            return this.productsRepository.All().Where(product => product.Barcodes.Any(b => b.Value == barcode));
        }

        public IQueryable<Product> GetByName(string name)
        {
            var nameAsArray = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return this.productsRepository.All().Where(x => nameAsArray.All(n => x.Name.Contains(n)));
        }

        public IQueryable<Product> GetByQuantity(decimal quantity)
        {
            return this.productsRepository.All().Where(pr => pr.Quantity <= quantity);
        }

        public void AddOrUpdate(string name, string oldName, decimal? buyingPrice, decimal? sellingPrice,
            MeasureType? measure, decimal? quantity, ICollection<Barcode> barcodes, ICollection<ExpirationDate> expirationDates)
        {
            if (oldName != name && this.productsRepository.All().Any(x => x.Name == name))
            {
                throw new ArgumentException($"A product with name {name} already exists");
            }

            var product = this.productsRepository.All().FirstOrDefault(x => x.Name == oldName);
            if (product == null)
            {
                product = new Product();
                this.productsRepository.Add(product);
            }

            var stockPrice = this.CalculateStockPrice(product, sellingPrice ?? product.SellingPrice, quantity ?? 0);

            this.MapProperties(product, name, buyingPrice, sellingPrice, measure, quantity, barcodes, expirationDates);

            if (quantity.GetValueOrDefault() != 0)
            {
                this.reportsService.AddShipment(product, quantity.GetValueOrDefault());
            }
            this.reportsService.AddStockPrice(stockPrice);
            this.productsRepository.SaveChanges();
        }

        private decimal CalculateStockPrice(Product product, decimal sellingPrice, decimal quantity)
        {
            decimal stockPrice = 0;

            var sellingPriceDifference = sellingPrice - product.SellingPrice;
            stockPrice += sellingPriceDifference * product.Quantity;
            stockPrice += sellingPrice * quantity;

            return stockPrice;
        }

        private void MapProperties(Product product, string name, decimal? buyingPrice, decimal? sellingPrice,
                                   MeasureType? measure, decimal? quantity, ICollection<Barcode> barcodes, ICollection<ExpirationDate> expirationDates)
        {
            product.Name = name;
            product.BuyingPrice = buyingPrice ?? product.BuyingPrice;
            product.SellingPrice = sellingPrice ?? product.SellingPrice;
            product.Measure = measure ?? product.Measure;
            product.Quantity += quantity ?? 0;

            foreach (var barcode in product.Barcodes.ToList())
            {
                if (barcodes.All(x => x.Value != barcode.Value))
                {
                    this.barcodesRepository.Delete(barcode);
                }
            }

            foreach (var barcode in barcodes)
            {
                if (!this.barcodesRepository.All().Any(x => x.Value == barcode.Value))
                {
                    product.Barcodes.Add(barcode);
                }
            }

            foreach (var expirationDate in expirationDates)
            {
                if (product.ExpirationDates.All(x => x.Date != expirationDate.Date || x.Batch != expirationDate.Batch))
                {
                    product.ExpirationDates.Add(expirationDate);
                }
            }
        }
    }
}