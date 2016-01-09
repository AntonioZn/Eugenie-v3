namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Models;

    public interface IServerManager
    {
        event EventHandler ServerTestingFinished;

        IProductsCache Cache { get; set; }

        Task AddOrUpdateAsync(IDictionary<ServerInformation, ProductViewModel> serverProductPairs);

        void Initialize();
    }
}