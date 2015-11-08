namespace Eugenie.Services.Data.Contracts
{
    using System.Linq;

    using Common.Constants;

    using Eugenie.Data.Models;

    public interface IProductsService
    {
        void Delete(int id);

        int Count();

        IQueryable<Product> All(int page, int pageSize = GlobalConstants.DefaultProductsPageSize);

        IQueryable<Product> FindById(int id);

        IQueryable<Product> FindById(string name);

        IQueryable<Product> FindByBarcode(string barcode);
    }
}