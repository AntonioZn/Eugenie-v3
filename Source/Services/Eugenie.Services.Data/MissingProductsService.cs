namespace Eugenie.Services.Data
{
    using System.Linq;

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
    }
}
