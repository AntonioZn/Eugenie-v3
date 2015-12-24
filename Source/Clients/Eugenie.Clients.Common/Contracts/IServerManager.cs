namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IServerManager
    {
        event EventHandler ServerTestingFinished;

        IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        void AddOrUpdateAsync(IDictionary<ServerInformation, Product> serverProductPairs);

        void AddServer(ServerInformation server);

        void DeleteServer(ServerInformation server);

        Task<IDictionary<ServerInformation, Product>> GetProductByNameAsync(string name);

        Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize);

        Task<int> GetProductsCount();

        void Initialize();

        bool HasActiveServer { get; }
    }
}