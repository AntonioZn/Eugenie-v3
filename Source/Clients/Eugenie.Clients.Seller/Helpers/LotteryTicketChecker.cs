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
            this.timer.Interval = 360000;
            this.timer.Elapsed += this.OnTimer;
        }

        public bool IsLoggedIn { get; private set; }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.LoginAsync(this.usernameCache, this.passwordCache, CancellationToken.None);
            }
            catch (Exception ex)
            {
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

            var response = await this.client.GetAsync($"https://7777.bg/loto_games/check_talon/?talon_id={barcode}", cToken);
            var result = await response.Content.ReadAsStringAsync();
            var document = this.parser.Parse(result);
            var winning = document.GetElementsByClassName("winnig-text").FirstOrDefault();
            if (winning != null)
            {
                return winning.TextContent;
            }

            var status = document.GetElementsByClassName("bordered").FirstOrDefault()?.GetElementsByTagName("h1").FirstOrDefault();
            if (status != null)
            {
                return status.TextContent;
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