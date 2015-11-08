namespace Eugenie.Services.Data
{
    using System.Linq;

    using Common.Constants;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    internal class ProductsService : IProductsService
    {
        private readonly IRepository<Product> productsRepository;
        private readonly IRepository<Barcode> barcodesRepository;

        public ProductsService(IRepository<Product> productsRepository, IRepository<Barcode> barcodesRepository)
        {
            this.productsRepository = productsRepository;
            this.barcodesRepository = barcodesRepository;
        }

        public void Delete(int id)
        {
            this.productsRepository.Delete(id);
        }

        public int Count()
        {
            return this.productsRepository.All().Count();
        }

        public IQueryable<Product> All(int page, int pageSize = GlobalConstants.DefaultProductsPageSize)
        {
            return this.productsRepository.All().OrderBy(pr => pr.Id).Skip((page - 1) * pageSize).Take(pageSize);
        }

        public IQueryable<Product> FindById(int id)
        {
            return this.productsRepository.All().Where(pr => pr.Id == id);
        }

        // TODO: Implement better searching algorithm
        // var nameAsArray = name.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        // var result =
        //     context.Products.Where(x => nameAsArray.All(n => x.Name.ToLower().Contains(n)))
        //         .OrderByDescending(x => nameAsArray.Any(n => x.Name.StartsWith(n)))
        //         .ToList();
        //         return result;
        public IQueryable<Product> FindById(string name)
        {
            return this.productsRepository.All().Where(pr => pr.Name.Contains(name));
        }

        // TODO: Implement
        public IQueryable<Product> FindByBarcode(string barcode)
        {
            return this.barcodesRepository.All().Where(bar => bar.Value.Contains(barcode)).Select(x => x.Product);
        }
    }
}