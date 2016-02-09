namespace Eugenie.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;

    using Common.Constants;
    using Common.Helpers;

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
            if (product != null)
            {
                product.IsDeleted = true;
                product.Name = $"[Изтрит на {DateTime.Now.ToString("dd-MM-yyyy")}]" + product.Name;
                var substringLenght = ValidationConstants.ProductNameMaxLength;
                if (product.Name.Length < substringLenght)
                {
                    substringLenght = product.Name.Length;
                }

                product.Name = product.Name.Substring(0, substringLenght);
                this.productsRepository.SaveChanges();
            }
        }

        public int Count()
        {
            return this.productsRepository.All().Count(x => !x.IsDeleted);
        }

        public IQueryable<Product> All()
        {
            return this.productsRepository.All().Include("Barcodes").Include("ExpirationDates").Where(x => !x.IsDeleted);
        }

        //TODO: if not product is found add it to missing products
        public IQueryable<Product> GetByBarcode(string barcode)
        {
            return this.productsRepository.All().Where(x => !x.IsDeleted).Where(product => product.Barcodes.Any(b => b.Value == barcode));
        }

        public IQueryable<Product> GetByName(string name)
        {
            var nameAsArray = name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return this.productsRepository.All().Where(x => !x.IsDeleted).Where(x => nameAsArray.All(n => x.Name.Contains(n)));
        }

        public IQueryable<Product> GetById(int id)
        {
            return this.productsRepository.All().Where(x => !x.IsDeleted).Where(x => x.Id == id);
        }

        public IQueryable<Product> GetByQuantity(decimal quantity)
        {
            return this.productsRepository.All().Where(x => !x.IsDeleted).Where(pr => pr.Quantity <= quantity);
        }

        public IQueryable<Product> GetByExpirationDate(int days)
        {
            var border = DateTime.Now.Date.AddDays(days * -1);
            return this.productsRepository.All().Where(x => !x.IsDeleted).Where(x => x.ExpirationDates.Any(date => date.Date <= border));
        }

        public void AddOrUpdate(string name, string oldName, decimal? buyingPrice, decimal? sellingPrice,
            MeasureType? measure, decimal? quantity, ICollection<Barcode> barcodes, ICollection<ExpirationDate> expirationDates)
        {
            if (oldName != name && this.productsRepository.All().Where(x => !x.IsDeleted).Any(x => x.Name == name))
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
            product.Name = ProductNameFixer.Fix(name);
            product.BuyingPrice = buyingPrice ?? product.BuyingPrice;
            product.SellingPrice = sellingPrice ?? product.SellingPrice;
            product.Measure = measure ?? product.Measure;
            product.Quantity += quantity ?? 0;

            foreach (var barcode in product.Barcodes.Where(barcode => barcodes.All(x => x.Value != barcode.Value)).ToList())
            {
                this.barcodesRepository.Delete(barcode);
            }
            
            foreach (var barcode in barcodes.Where(barcode => !this.barcodesRepository.All().Any(x => x.Value == barcode.Value)))
            {
                product.Barcodes.Add(barcode);
            }

            foreach (var expirationDate in expirationDates.Where(expirationDate => product.ExpirationDates.All(x => x.Date != expirationDate.Date || x.Batch != expirationDate.Batch)))
            {
                product.ExpirationDates.Add(expirationDate);
            }
        }
    }
}