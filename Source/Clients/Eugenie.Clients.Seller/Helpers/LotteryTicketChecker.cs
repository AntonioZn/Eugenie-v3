namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Timers;

    using AngleSharp.Parser.Html;

    using Newtonsoft.Json.Linq;

    using Sv.Wpf.Core.Extensions;
    using Sv.Wpf.Core.Helpers;

    using Timer = System.Timers.Timer;

    public class LotteryTicketChecker : IDisposable
    {
        private readonly HtmlParser parser = new HtmlParser();
        private readonly Timer timer;

        private ResilientHttpClient client;

        private string usernameCache;
        private string passwordCache;

        public LotteryTicketChecker()
        {
            this.timer = new Timer();
            this.timer.Interval = 300000;
            this.timer.Elapsed += this.OnTimer;
        }

        public bool IsLoggedIn { get; private set; }

        private async void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                await this.LoginAsync(this.usernameCache, this.passwordCache, CancellationToken.None);
            }
            catch (Exception)
            {
                this.IsLoggedIn = false;
            }
        }

        public async Task<bool> LoginAsync(string username, string password, CancellationToken cToken)
        {
            this.usernameCache = username;
            this.passwordCache = password;

            var newClient = await this.GetClientAndLoginAsync(this.usernameCache, this.passwordCache, cToken);
            this.client?.Dispose();
            this.client = newClient;

            if (newClient == null)
            {
                this.IsLoggedIn = false;
                return false;
            }
            else
            {
                this.IsLoggedIn = true;
                this.timer.Start();
                return true;
            }
        }

        private async Task<ResilientHttpClient> GetClientAndLoginAsync(string username, string password, CancellationToken cToken)
        {
            var client = new ResilientHttpClient();
            client.RetryAfterTimeout = false;
            client.RetryCount = 0;
            client.Timeout = TimeSpan.FromSeconds(10);

            var content = new FormUrlEncodedContent(new[]
                                                    {
                                                        new KeyValuePair<string, string>("login_username", username),
                                                        new KeyValuePair<string, string>("login_password", password),
                                                        new KeyValuePair<string, string>("submit_form", "1")
                                                    });

            var response = await client.PostAsync("https://7777.bg/login/", content, cToken);
            var result = await response.Content.ReadAsStringAsync();
            var document = this.parser.Parse(result);
            var title = document.Title;
            if (title.ToLower().Contains("вход"))
            {
                return null;
            }

            return client;
        }

        public async Task<string> CheckAsync(string barcode, CancellationToken cToken)
        {
            if (barcode.Length != 14 && barcode.Length != 13)
            {
                return "Невалиден баркод";
            }

            var request = new FormUrlEncodedContent(new Dictionary<string, string>
                                                    {
                                                        {"talon_id_mobile", ""},
                                                        {"talon_id", barcode},
                                                        {"environment", "desktop"},
                                                        {"is_ajax", "1"},
                                                    });

            var response = await this.client.PostAsync("https://7777.bg/check_winning/", request, cToken);
            var json = await response.Content.ReadAsStringAsync();
            var jObject = JObject.Parse(json);
            var html = jObject["html"]["popup"].ToString();
            var document = this.parser.Parse(html);

            var errors = document.GetElementsByClassName("popup-error-text");
            if (errors.Any())
            {
                return errors.First().TextContent.Trim().TrimMultipleWhiteSpaces();
            }

            var winning = document.GetElementsByClassName("popup-hdr-winbox flex flow-row flex-wrap");
            if (winning.Any())
            {
                return winning.First().GetElementsByTagName("p").Last().TextContent.Trim();
            }

            return "Грешка";
        }

        public void Dispose()
        {
            this.timer?.Dispose();
            this.client?.Dispose();
        }
    }
}