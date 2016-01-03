namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IServerManager
    {
        IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        event EventHandler ServerTestingFinished;

        void AddOrUpdateAsync(IDictionary<ServerInformation, Product> serverProductPairs);

        Task<IDictionary<ServerInformation, Product>> GetProductByNameAsync(string name);

        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize);

        Task<int> GetProductsCount();

        void TestServers();
    }
}