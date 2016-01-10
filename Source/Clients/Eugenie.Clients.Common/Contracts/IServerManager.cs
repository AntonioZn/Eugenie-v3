namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Models;

    public interface IServerManager
    {
        event EventHandler ServerTestingFinished;

        IProductsCache Cache { get; set; }

        Task AddOrUpdateAsync(ServerInformation server, AddProductModel model);

        void Initialize();
    }
}