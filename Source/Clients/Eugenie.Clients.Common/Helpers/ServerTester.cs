﻿namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Diagnostics;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Contracts;

    using Models;

    using Newtonsoft.Json;

    public class ServerTester : IServerTester
    {
        public async Task<HttpClient> TestServer(ServerInformation server)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.BaseAddress = new Uri(server.Address);

            var result = await this.GetToken(client, server);
            if (result)
            {
                return client;
            }

            return null;
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