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
        private readonly IReportsService reportsService;
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<User> sellersRepository;
        private readonly IRepository<Sell> sellsRepository;
        private readonly IRepository<Waste> wasteRepository;

        public DealsService(IRepository<Product> productsRepository, IRepository<User> sellersRepository,
                            IRepository<Sell> sellsRepository, IRepository<Waste> wasteRepository, IReportsService reportsService)
        {
            this.productsRepository = productsRepository;
            this.sellersRepository = sellersRepository;
            this.sellsRepository = sellsRepository;
            this.wasteRepository = wasteRepository;
            this.reportsService = reportsService;
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
            var report = this.reportsService.GetTodaysReport();

            sell.Seller = seller;
            sell.Report = report;

            this.sellsRepository.Add(sell);
            this.sellsRepository.SaveChanges();
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
            var report = this.reportsService.GetTodaysReport();

            waste.Seller = seller;
            waste.Report = report;

            this.wasteRepository.Add(waste);
            this.wasteRepository.SaveChanges();
        }

        public IQueryable<Sell> GetSells(string name, DateTime start, DateTime end)
        {
            return this.sellsRepository.All().Where(x => x.Seller.UserName == name && x.Date.CompareTo(start) >= 0 && x.Date.CompareTo(end) <= 0);
        }

        public IQueryable<Waste> GetWaste(string name, DateTime start, DateTime end)
        {
            return this.wasteRepository.All().Where(x => x.Seller.UserName == name && x.Date.CompareTo(start) >= 0 && x.Date.CompareTo(end) <= 0);
        }
    }
}