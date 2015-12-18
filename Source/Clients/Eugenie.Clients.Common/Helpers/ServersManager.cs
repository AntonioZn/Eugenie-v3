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

    using Newtonsoft.Json;

    public class ServersManager : IServersManager
    {
        public ServersManager()
        {
            this.ActiveServers = new SortedDictionary<ServerInformation, HttpClient>();
        }

        public IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }

        public HttpClient FastestServer => this.ActiveServers.First().Value;

        public async Task<IDictionary<ServerInformation, HttpClient>> TestServers(IEnumerable<ServerInformation> servers)
        {
            this.ActiveServers.Clear();

            foreach (var server in servers)
            {
                var client = new HttpClient { BaseAddress = server.Uri };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = server.Uri;

                var result = await this.GetToken(client, server);
                if (result)
                {
                    this.ActiveServers.Add(server, client);
                }
            }

            if (this.ActiveServers.Any())
            {
                Messenger.Default.Send(new ServerTestingFinishedMessage());
            }

            return this.ActiveServers;
        }

        private async Task<bool> GetToken(HttpClient client, ServerInformation server)
        {
            try
            {
                var stopWatch = Stopwatch.StartNew();
                var content = new StringContent($"grant_type=password&username={server.Username}&password={server.Password}");
                var response = await client.PostAsync("api/account/token", content);
                var result = await response.Content.ReadAsStringAsync();
                var deserializedResult = JsonConvert.DeserializeObject<LoginTokenResponse>(result);
                server.AuthToken = deserializedResult.access_token;
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", server.AuthToken);
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