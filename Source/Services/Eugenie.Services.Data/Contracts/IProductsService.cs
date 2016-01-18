namespace Eugenie.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(string name);

        int Count();

        Product AddOrUpdate(string name, string newName, decimal? buyingPrice, decimal? sellingPrice, MeasureType? measure, decimal? quantity, ICollection<Barcode> barcodes, ICollection<ExpirationDate> expirationDates);

        IQueryable<Product> All();

        Product FindByName(string name);

        IQueryable<Product> FindByQuantity(decimal quantity);
    }
}