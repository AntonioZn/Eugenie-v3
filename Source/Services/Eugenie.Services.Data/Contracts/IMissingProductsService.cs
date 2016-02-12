namespace Eugenie.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IMissingProductsService
    {
        IQueryable<MissingProduct> GetMissingProducts();

        void AddMissingProduct(string barcode);

        void RemoveMissingProducts(IEnumerable<string> barcodes);
    }
}