namespace Eugenie.Services.Data
{
    using System;
    using System.Linq;

    using Common.Helpers;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    public class MissingProductsService : IMissingProductsService
    {
        private readonly IRepository<MissingProduct> missingProductsRepository;

        public MissingProductsService(IRepository<MissingProduct> missingProductsRepository)
        {
            this.missingProductsRepository = missingProductsRepository;
        }

        public IQueryable<MissingProduct> GetMissingProducts()
        {
            return this.missingProductsRepository.All();
        }

        public void AddMissingProduct(string barcode)
        {
            if (this.missingProductsRepository.All().All(x => x.Barcode != barcode))
            {
                var missingProduct = new MissingProduct();
                missingProduct.Barcode = barcode;
                missingProduct.Date = DateTime.Now;
                missingProduct.Name = NameFromBarcodeGenerator.GetNameAsync(barcode).Result;
                this.missingProductsRepository.Add(missingProduct);
                this.missingProductsRepository.SaveChanges();
            }
        }
    }
}
