namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Models;

    using WebApiModels;

    public interface IServerManager
    {
        event EventHandler ServerTestingFinished;

        ProductsCache Cache { get; set; }

        Task AddOrUpdateAsync(ServerInformation server, AddProductModel model);

        void Initialize();
    }
}