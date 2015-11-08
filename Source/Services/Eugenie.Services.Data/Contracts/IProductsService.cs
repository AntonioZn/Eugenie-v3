namespace Eugenie.Services.Data.Contracts
{
    using System;
    using System.Linq;

    using Common.Constants;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(Product product);

        int Count();

        Product Add(string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity, string barcode, DateTime? expirationDate);

        IQueryable<Product> All(int page, int pageSize = GlobalConstants.ProductsPageSize);

        Product Update(Product product, string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity);

        Product AddBarcode(Product product, string barcode);

        Product AddExpirationDate(Product product, DateTime expirationDate);

        IQueryable<Product> FindById(int id);

        IQueryable<Product> FindByName(string name);

        IQueryable<Product> FindByBarcode(string barcode);
        
        IQueryable<Product> FindByQuantity(decimal quantity);
    }
}