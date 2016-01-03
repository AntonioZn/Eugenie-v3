namespace Eugenie.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Common.Constants;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(string name);

        int Count();

        Product AddOrUpdate(string name, string newName, decimal? buyingPrice, decimal? sellingPrice, MeasureType? measure, decimal? quantity, IEnumerable<Barcode> barcodes, IEnumerable<ExpirationDate> expirationDates);

        IQueryable<Product> All(int page, int pageSize = GlobalConstants.ProductsPageSize);

        Product FindByName(string name);

        IQueryable<Product> FindByQuantity(decimal quantity);
    }
}