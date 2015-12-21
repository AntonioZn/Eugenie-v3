namespace Eugenie.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Constants;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Barcode> barcodesRepository;
        private readonly IDailyEarningsService dailyEarningsService;
        private readonly IRepository<Product> productsRepository;

        public ProductsService(IRepository<Product> productsRepository, IRepository<Barcode> barcodesRepository, IDailyEarningsService dailyEarningsService)
        {
            this.productsRepository = productsRepository;
            this.barcodesRepository = barcodesRepository;
            this.dailyEarningsService = dailyEarningsService;
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

        //TODO: add stockprice
        public Product AddOrUpdate(string name, string oldName, decimal buyingPrice, decimal sellingPrice, 
            MeasureType measure, decimal quantity, IEnumerable<string> barcodes, IEnumerable<DateTime> expirationDates)
        {
            if (name != oldName && this.productsRepository.All().Any(x => x.Name == name))
            {
                throw new ArgumentException($"A product with name {name} already exists");
            }

            if (oldName != null)
            {
                var oldProduct = this.productsRepository.All().FirstOrDefault(x => x.Name == oldName);
                if (oldProduct != null)
                {
                    oldProduct.Name = name;
                    oldProduct.BuyingPrice = buyingPrice;
                    oldProduct.SellingPrice = sellingPrice;
                    oldProduct.Measure = measure;
                    oldProduct.Quantity += quantity;
                    
                    foreach (var barcode in barcodes)
                    {
                        if (!this.barcodesRepository.All().Any(x => x.Value == barcode))
                        {
                            var barcodeObj = new Barcode { Value = barcode };
                            oldProduct.Barcodes.Add(barcodeObj);
                        }
                    }
                    
                    foreach (var expirationDate in expirationDates)
                    {
                        if (oldProduct.ExpirationDates.All(x => x.Date != expirationDate))
                        {
                            var expDateObj = new ExpirationDate { Date = expirationDate };
                            oldProduct.ExpirationDates.Add(expDateObj);
                        }
                    }
                    
                    this.productsRepository.SaveChanges();
                    return oldProduct;
                }
            }

            var newProduct = new Product
                          {
                              Name = name,
                              BuyingPrice = buyingPrice,
                              SellingPrice = sellingPrice,
                              Measure = measure,
                              Quantity = quantity
                          };
            
            foreach (var barcode in barcodes)
            {
                if (!this.barcodesRepository.All().Any(x => x.Value == barcode))
                {
                    var barcodeObj = new Barcode { Value = barcode };
                    newProduct.Barcodes.Add(barcodeObj);
                }
            }
            
            foreach (var expirationDate in expirationDates)
            {
                if (newProduct.ExpirationDates.All(x => x.Date != expirationDate))
                {
                    var expDateObj = new ExpirationDate { Date = expirationDate };
                    newProduct.ExpirationDates.Add(expDateObj);
                }
            }

            this.productsRepository.Add(newProduct);
            this.productsRepository.SaveChanges();

            return newProduct;
        }

        public IQueryable<Product> All(int page, int pageSize = GlobalConstants.ProductsPageSize)
        {
            return this.productsRepository.All().OrderBy(pr => pr.Id).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Product FindByName(string name)
        {
            return this.productsRepository.All().FirstOrDefault(x => x.Name == name);
        }

        public IQueryable<Product> FindByQuantity(decimal quantity)
        {
            return this.productsRepository.All().Where(pr => pr.Quantity <= quantity);
        }
    }
}