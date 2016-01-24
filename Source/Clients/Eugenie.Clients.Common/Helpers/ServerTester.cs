namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;

    using Models;

    using Newtonsoft.Json;

    using WebApiModels;

    public static class ServerTester
    {
        public static async Task<HttpClient> TestServerAsync(ServerInformation server)
        {
            foreach (var address in server.Addresses)
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.BaseAddress = new Uri(address);

                var result = await GetTokenAsync(client, server);
                if (result)
                {
                    return client;
                }
            }

            return null;
        }

        private static async Task<bool> GetTokenAsync(HttpClient client, ServerInformation server)
        {
            try
            {
                var content = new StringContent($"grant_type=password&username={server.Username}&password={server.Password}");
                var response = await client.PostAsync("api/account/token", content);
                var result = await response.Content.ReadAsStringAsync();
                var deserializedResult = JsonConvert.DeserializeObject<LoginTokenResponse>(result);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", deserializedResult.access_token);
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