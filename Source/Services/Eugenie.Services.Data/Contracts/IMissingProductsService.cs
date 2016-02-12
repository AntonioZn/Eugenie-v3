namespace Eugenie.Services.Data.Contracts
{
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IMissingProductsService
    {
        IQueryable<MissingProduct> GetMissingProducts();

        void AddMissingProduct(string barcode);
    }
}