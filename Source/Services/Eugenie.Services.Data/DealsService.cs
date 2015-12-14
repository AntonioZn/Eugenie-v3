namespace Eugenie.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;
    using Eugenie.Data.Models.DummyModels;

    public class DealsService : IDealsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<DailyEarning> dailyEarningsRepository;
        private readonly IRepository<User> sellersRepository;

        public DealsService(IRepository<Product> productsRepository, IRepository<DailyEarning> dailyEarningsRepository, IRepository<User> sellersRepository)
        {
            this.productsRepository = productsRepository;
            this.dailyEarningsRepository = dailyEarningsRepository;
            this.sellersRepository = sellersRepository;
        }
        
        public void Sell(string sellerId, IEnumerable<IdQuantityPair> products)
        {
            var sell = new Sell();

            foreach (var product in products)
            {
                var soldProduct = this.productsRepository.GetById(product.Id);

                if (soldProduct == null)
                {
                    throw new ArgumentException($"Product with Id {product.Id} does not exist.", "Id");
                }

                var productWithQuantity = new ProductWithQuantity
                                          {
                                              Product = soldProduct,
                                              Quantity = product.Quantity
                                          };

                sell.AddProduct(productWithQuantity);

                soldProduct.Quantity -= product.Quantity;
            }
            
            var seller = this.sellersRepository.GetById(sellerId);
            seller.Sells.Add(sell);
            this.sellersRepository.SaveChanges();

            var dailyEarning = this.dailyEarningsRepository.All().FirstOrDefault(x => x.Date == DateTime.Today);
            
            if (dailyEarning == null)
            {
                dailyEarning = new DailyEarning();
                this.dailyEarningsRepository.Add(dailyEarning);
            }
            dailyEarning.Earnings += sell.Total;

            this.dailyEarningsRepository.SaveChanges();
            this.productsRepository.SaveChanges();
        }
        
        public void Waste(string sellerId, IEnumerable<IdQuantityPair> products)
        {
            var waste = new Waste();

            foreach (var product in products)
            {
                var wastedProduct = this.productsRepository.GetById(product.Id);

                if (wastedProduct == null)
                {
                    throw new ArgumentException($"Product with Id {product.Id} does not exist.", "Id");
                }

                var productWithQuantity = new ProductWithQuantity
                {
                    Product = wastedProduct,
                    Quantity = product.Quantity
                };

                waste.AddProduct(productWithQuantity);

                wastedProduct.Quantity -= product.Quantity;
            }

            var seller = this.sellersRepository.GetById(sellerId);
            seller.Waste.Add(waste);
            this.sellersRepository.SaveChanges();

            var dailyEarning = this.dailyEarningsRepository.All().FirstOrDefault(x => x.Date == DateTime.Today);

            if (dailyEarning == null)
            {
                dailyEarning = new DailyEarning();
                this.dailyEarningsRepository.Add(dailyEarning);
            }
            dailyEarning.Waste += waste.Total;

            this.dailyEarningsRepository.SaveChanges();
            this.productsRepository.SaveChanges();
        }
    }
}