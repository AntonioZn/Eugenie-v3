namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    public class ServerManager : IServerManager
    {
        private readonly IServerStorage storage;
        private readonly IServerTester tester;
        private readonly IWebApiClient webApiClient;

        public ServerManager(IServerStorage storage, IServerTester tester, IWebApiClient webApiClient)
        {
            this.storage = storage;
            this.tester = tester;
            this.webApiClient = webApiClient;

            this.ActiveServers = new Dictionary<ServerInformation, HttpClient>();

            this.Initialize();
            //var inactiveServers = storage.Servers.Except(this.ActiveServers.Keys);
        }

        public event EventHandler ServerTestingFinished;

        public bool HasActiveServer => this.ActiveServers.Any();

        public IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        public async Task<int> GetProductsCount()
        {
            return await this.webApiClient.GetProductsCountAsync(this.GetFastestServer());
        }

        public async Task<IEnumerable<SimplifiedProduct>> GetProductsByPageAsync(int page, int pageSize)
        {
            return await this.webApiClient.GetProductsByPageAsync(this.GetFastestServer(), page, pageSize);
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

        public async void AddServer(ServerInformation server)
        {
            this.storage.AddServer(server);
            await this.TestServer(server);
        }
        
        public void DeleteServer(ServerInformation server)
        {
            this.storage.DeleteServer(server);
            var serverToDelete = this.ActiveServers.FirstOrDefault(x => x.Key.Name == server.Name);
            this.ActiveServers.Remove(serverToDelete);
        }

        public async void Initialize()
        {
            this.ActiveServers.Clear();
            foreach (var server in this.storage.Servers)
            {
                await this.TestServer(server);
            }

            this.OnServerTestingFinished(EventArgs.Empty);
        }

        private async Task TestServer(ServerInformation server)
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

        private HttpClient GetFastestServer()
        {
            ServerInformation fastestServer = this.ActiveServers.Keys.ToList()[0];
            foreach (var server in this.ActiveServers.Keys)
            {
                if (fastestServer.Ping > server.Ping)
                {
                    fastestServer = server;
                }
            }

            return this.ActiveServers[fastestServer];
        }
        
        private void OnServerTestingFinished(EventArgs e)
        {
            var handler = this.ServerTestingFinished;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}