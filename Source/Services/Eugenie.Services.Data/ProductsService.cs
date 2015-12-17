namespace Eugenie.Services.Data
{
    using System;
    using System.Linq;

    using Common.Constants;
    using Common.Helpers;

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

        public void Delete(int id)
        {
            this.productsRepository.Delete(id);
            this.productsRepository.SaveChanges();
        }

        public int Count()
        {
            return this.productsRepository.All().Count();
        }

        //TODO: add stockprice
        public Product Add(string name, decimal buyingPrice = 0, decimal sellingPrice = 0, MeasureType measure = MeasureType.бр, decimal quantity = 0, string barcode = null, DateTime? expirationDate = null)
        {
            if (this.productsRepository.All().FirstOrDefault(x => x.Name == name) != null)
            {
                throw new ArgumentException($"A product with name {name} already exists");
            }

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

        //TODO: add stockprice
        public Product Update(int productId, string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity)
        {
            var product = this.productsRepository.GetById(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with Id {productId} does not exist");
            }

            if (this.productsRepository.All().FirstOrDefault(x => x.Name == name) != null)
            {
                throw new ArgumentException($"A product with name {name} already exists");
            }

            product.Name = ProductNameFixer.Fix(name);
            product.BuyingPrice = buyingPrice;
            product.SellingPrice = sellingPrice;
            product.Measure = measure;

            product.Quantity += quantity;

            this.productsRepository.Update(product);
            this.productsRepository.SaveChanges();

            return product;
        }

        public Product AddBarcode(int productId, string barcode)
        {
            var product = this.productsRepository.GetById(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with Id {productId} does not exist");
            }

            if (this.barcodesRepository.All().FirstOrDefault(x => x.Value == barcode) != null)
            {
                throw new ArgumentException($"Barcode {barcode} already exists");
            }

            var barcodeObj = new Barcode { Value = barcode };
            product.Barcodes.Add(barcodeObj);

            this.productsRepository.Update(product);
            this.productsRepository.SaveChanges();

            return product;
        }

        public Product AddExpirationDate(int productId, DateTime expirationDate)
        {
            var product = this.productsRepository.GetById(productId);
            if (product == null)
            {
                throw new ArgumentException($"Product with Id {productId} does not exist");
            }

            if (product.ExpirationDates.FirstOrDefault(x => x.Date == expirationDate) != null)
            {
                throw new ArgumentException("The product already contains the given expiration date");
            }

            var expirationDateObj = new ExpirationDate { Date = expirationDate };
            product.ExpirationDates.Add(expirationDateObj);

            this.productsRepository.Update(product);
            this.productsRepository.SaveChanges();

            return product;
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