namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Contracts;

    using GalaSoft.MvvmLight.Messaging;

    using Messages;

    using Models;

    public class ServersManager : IServersManager
    {
        private readonly IEnumerable<ServerInformation> servers;

        public ServersManager(IEnumerable<ServerInformation> servers)
        {
            this.servers = servers;
            this.ActiveServers = new SortedDictionary<ServerInformation, HttpClient>();
        }

        public IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        public HttpClient FastestServer => this.ActiveServers.First().Value;

        public async Task<IDictionary<ServerInformation, HttpClient>> TestServers()
        {
            foreach (var server in this.servers)
            {
                var client = new HttpClient { BaseAddress = server.Uri };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = server.Uri;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", server.AuthToken);

                var result = await this.GetProductsCountAsync(client, server);
                if (result)
                {
                    this.ActiveServers.Add(server, client);
                }
            }

            Messenger.Default.Send(new ServerTestingFinishedMessage());
            return this.ActiveServers;
        }

        private async Task<bool> GetProductsCountAsync(HttpClient client, ServerInformation server)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var response = await client.GetAsync("api/products");
                server.Ping = stopWatch.Elapsed;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }
            catch
            {
            }

            return false;
        }
    }
}