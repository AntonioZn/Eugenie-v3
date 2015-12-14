namespace Eugenie.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Constants;
    using Common.Helpers;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;
    using Eugenie.Data.Models.DummyModels;

    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Barcode> barcodesRepository;

        public ProductsService(IRepository<Product> productsRepository, IRepository<Barcode> barcodesRepository)
        {
            this.productsRepository = productsRepository;
            this.barcodesRepository = barcodesRepository;
        }

        public void Delete(Product product)
        {
            this.productsRepository.Delete(product);
            this.productsRepository.SaveChanges();
        }

        public int Count()
        {
            return this.productsRepository.All().Count();
        }

        public Product Add(string name, decimal buyingPrice = 0, decimal sellingPrice = 0, MeasureType measure = MeasureType.бр, decimal quantity = 0, string barcode = null, DateTime? expirationDate = null)
        {
            var product = new Product
                          {
                              Name = name,
                              BuyingPrice = buyingPrice,
                              SellingPrice = sellingPrice,
                              Measure = measure,
                              Quantity = quantity
                          };

            if (!string.IsNullOrEmpty(barcode))
            {
                var barcodeObj = new Barcode { Value = barcode };
                product.Barcodes.Add(barcodeObj);
            }

            if (expirationDate != null)
            {
                var expirationDateObj = new ExpirationDate { Date = expirationDate.Value };
                product.ExpirationDates.Add(expirationDateObj);
            }

            this.productsRepository.Add(product);
            this.productsRepository.SaveChanges();

            return product;
        }

        public IQueryable<Product> All(int page, int pageSize = GlobalConstants.ProductsPageSize)
        {
            return this.productsRepository.All().OrderBy(pr => pr.Id).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public Product Update(Product product, string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity)
        {
            product.Name = ProductNameFixer.Fix(name);
            product.BuyingPrice = buyingPrice;
            product.SellingPrice = sellingPrice;
            product.Measure = measure;

            product.Quantity += quantity;
            
            this.productsRepository.Update(product);
            this.productsRepository.SaveChanges();
            
            return product;
        }

        public Product AddBarcode(Product product, string barcode)
        {
            if (this.barcodesRepository.All().FirstOrDefault(x => x.Value == barcode) == null)
            {
                var barcodeObj = new Barcode { Value = barcode };
                product.Barcodes.Add(barcodeObj);

                this.productsRepository.Update(product);
                this.productsRepository.SaveChanges();

                return product;
            }

            return null;
        }

        public Product AddExpirationDate(Product product, DateTime expirationDate)
        {
            if (product.ExpirationDates.FirstOrDefault(x => x.Date == expirationDate) == null)
            {
                var expirationDateObj = new ExpirationDate { Date = expirationDate };
                product.ExpirationDates.Add(expirationDateObj);

                this.productsRepository.Update(product);
                this.productsRepository.SaveChanges();

                return product;
            }

            return null;
        }

        public IQueryable<Product> FindById(int id)
        {
            return this.productsRepository.All().Where(pr => pr.Id == id);
        }

        // TODO: Implement better searching algorithm
        // var nameAsArray = name.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        // var result =
        //     context.Products.Where(x => nameAsArray.All(n => x.Name.ToLower().Contains(n)))
        //         .OrderByDescending(x => nameAsArray.Any(n => x.Name.StartsWith(n)))
        //         .ToList();
        //         return result;
        public IQueryable<Product> FindByName(string name)
        {
            return this.productsRepository.All().Where(pr => pr.Name.Contains(name));
        }
        
        public IQueryable<Product> FindByBarcode(string barcode)
        {
            return this.barcodesRepository.All().Where(bar => bar.Value.Contains(barcode)).Select(x => x.Product);
        }

        public IQueryable<Product> FindByQuantity(decimal quantity)
        {
            return this.productsRepository.All().Where(pr => pr.Quantity <= quantity);
        }
    }
}