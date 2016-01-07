namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;

    using Models;

    public interface IServerManager
    {
        event EventHandler ServerTestingFinished;

        IProductsCache Cache { get; set; }

        void AddOrUpdateAsync(IDictionary<ServerInformation, Product> serverProductPairs);

        void Initialize();
    }
}