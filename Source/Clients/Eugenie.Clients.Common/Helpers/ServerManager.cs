namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly IServerTester tester;
        private readonly IWebApiClient webApiClient;
        private readonly IProductsCache cache;
        private readonly SemaphoreSlim semaphore;

        public ServerManager(IServerStorage storage, IServerTester tester, IWebApiClient webApiClient, IProductsCache cache)
        {
            this.storage = storage;
            this.storage.ServerAdded += this.OnServerAdded;
            this.storage.ServerDeleted += this.OnServerDeleted;
            this.tester = tester;
            this.webApiClient = webApiClient;
            this.cache = cache;

            this.semaphore = new SemaphoreSlim(1);

            this.ActiveServers = new Dictionary<ServerInformation, HttpClient>();

            this.TestServers();
            //var inactiveServers = storage.Servers.Except(this.ActiveServers.Keys);
        }

        public IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        #region WebApi

        public async Task<int> GetProductsCount()
        {
            return await this.webApiClient.GetProductsCountAsync(this.GetFastestServer());
        }

        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize)
        {
            await this.semaphore.WaitAsync();
            if (this.cache.SimplifiedProducts != null)
            {
                this.semaphore.Release();
                return this.cache.SimplifiedProducts;
            }

            this.cache.SimplifiedProducts = await this.webApiClient.GetProductsByPageAsync(this.GetFastestServer(), page, pageSize);

            this.semaphore.Release();

            return this.cache.SimplifiedProducts;
        }

        public async Task<IDictionary<ServerInformation, Product>> GetProductByNameAsync(string name)
        {
            var result = new Dictionary<ServerInformation, Product>();

            foreach (var pair in this.ActiveServers)
            {
                var product = await this.webApiClient.GetProductByName(pair.Value, name);
                result.Add(pair.Key, product);
            }

            return result;
        }

        public void AddOrUpdateAsync(IDictionary<ServerInformation, Product> serverProductPairs)
        {
            foreach (var pair in serverProductPairs)
            {
                var client = this.ActiveServers[pair.Key];
                this.webApiClient.AddOrUpdateAsync(client, pair.Value);
            }
        }
        #endregion

        public event EventHandler ServerTestingFinished;

        public async void TestServers()
        {
            this.ActiveServers.Clear();
            foreach (var server in this.storage.Servers)
            {
                try
                {
                    var client = await this.tester.TestServer(server);
                    this.ActiveServers.Add(server, client);
                }
                catch (ArgumentException ex)
                {

                }
            }

            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }

        private HttpClient GetFastestServer()
        {
            var fastestServer = this.ActiveServers.Keys.ToList()[0];
            foreach (var server in this.ActiveServers.Keys)
            {
                if (fastestServer.Ping > server.Ping)
                {
                    fastestServer = server;
                }
            }

            return this.ActiveServers[fastestServer];
        }

        private async void OnServerAdded(object sender, ServerAddedEventArgs e)
        {
            try
            {
                var client = await this.tester.TestServer(e.Server);
                this.ActiveServers.Add(e.Server, client);
            }
            catch (ArgumentException ex)
            {

            }
        }

        private void OnServerDeleted(object sender, ServerDeletedEventArgs e)
        {
            if (this.ActiveServers.ContainsKey(e.Server))
            {
                this.ActiveServers.Remove(e.Server);
            }
        }
    }
}