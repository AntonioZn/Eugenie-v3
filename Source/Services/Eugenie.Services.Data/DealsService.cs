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
        private readonly IDailyEarningsService dailyEarningsService;
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<User> sellersRepository;
        private readonly IRepository<Sell> sellsRepository;
        private readonly IRepository<Waste> wasteRepository;

        public DealsService(IRepository<Product> productsRepository, IRepository<User> sellersRepository,
                            IRepository<Sell> sellsRepository, IRepository<Waste> wasteRepository, IDailyEarningsService dailyEarningsService)
        {
            this.productsRepository = productsRepository;
            this.sellersRepository = sellersRepository;
            this.sellsRepository = sellsRepository;
            this.wasteRepository = wasteRepository;
            this.dailyEarningsService = dailyEarningsService;
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

            this.dailyEarningsService.AddSell(sell.Total);
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

            this.dailyEarningsService.AddWaste(waste.Total);
            this.productsRepository.SaveChanges();
        }

        public IEnumerable<Sell> GetSells(string sellerId, string start, string end)
        {
            var startDate = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.Parse(start);
            var endDate = string.IsNullOrEmpty(end) ? DateTime.MaxValue.AddDays(-1) : DateTime.Parse(end);
            endDate = endDate.AddDays(1);

            if (string.IsNullOrEmpty(sellerId))
            {
                return this.sellsRepository.All().Where(x => x.Date.CompareTo(startDate) >= 0 && x.Date.CompareTo(endDate) <= 0).ToList();
            }

            var seller = this.sellersRepository.GetById(sellerId);
            if (seller == null)
            {
                throw new ArgumentException($"Seller with Id {sellerId} does not exist", "SellerId");
            }

            return seller.Sells.Where(x => x.Date.CompareTo(startDate) >= 0 && x.Date.CompareTo(endDate) <= 0);
        }

        public IEnumerable<Waste> GetWaste(string sellerId, string start, string end)
        {
            var startDate = string.IsNullOrEmpty(start) ? DateTime.MinValue : DateTime.Parse(start);
            var endDate = string.IsNullOrEmpty(end) ? DateTime.MaxValue.AddDays(-1) : DateTime.Parse(end);
            endDate = endDate.AddDays(1);

            if (string.IsNullOrEmpty(sellerId))
            {
                return this.wasteRepository.All().Where(x => x.Date.CompareTo(startDate) >= 0 && x.Date.CompareTo(endDate) <= 0).ToList();
            }

            var seller = this.sellersRepository.GetById(sellerId);
            if (seller == null)
            {
                throw new ArgumentException($"Seller with Id {sellerId} does not exist", "SellerId");
            }

            return seller.Waste.Where(x => x.Date.CompareTo(startDate) >= 0 && x.Date.CompareTo(endDate) <= 0);
        }
    }
}