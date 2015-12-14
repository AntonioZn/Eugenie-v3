namespace Eugenie.Services.Data.Contracts
{
    using System;
    using System.Linq;

    using Common.Constants;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(int id);

        int Count();

        Product Add(string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity, string barcode, DateTime? expirationDate);

        IQueryable<Product> All(int page, int pageSize = GlobalConstants.ProductsPageSize);

        Product Update(int productId, string name, decimal buyingPrice, decimal sellingPrice, MeasureType measure, decimal quantity);

        Product AddBarcode(int productId, string barcode);

        Product AddExpirationDate(int productId, DateTime expirationDate);

        IQueryable<Product> FindById(int id);

        IQueryable<Product> FindByName(string name);

        IQueryable<Product> FindByBarcode(string barcode);
        
        IQueryable<Product> FindByQuantity(decimal quantity);
    }
}