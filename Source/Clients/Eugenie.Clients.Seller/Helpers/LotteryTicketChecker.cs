namespace Eugenie.Clients.Seller.Helpers
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    using AngleSharp.Parser.Html;

    using Annotations;

    public class LotteryTicketChecker : INotifyPropertyChanged
    {
        private readonly HtmlParser parser = new HtmlParser();
        private CookieContainer cookieContainer = new CookieContainer();
        private bool isLoggedIn;

        public bool IsLoggedIn
        {
            get
            {
                return this.isLoggedIn;
            }
            private set
            {
                this.isLoggedIn = value;
                this.OnPropertyChanged();
            }
        }

        public async Task<bool> Login(string username, string password)
        {
            this.cookieContainer = new CookieContainer();

            var handler = new HttpClientHandler();
            handler.CookieContainer = this.cookieContainer;

            using (var client = new HttpClient(handler))
            {
                var content = new FormUrlEncodedContent(new[] {
                                                                        new KeyValuePair<string, string>("login_username", username),
                                                                        new KeyValuePair<string, string>("login_password", password),
                                                                        new KeyValuePair<string, string>("submit_form", "1")
                                                              });

                var response = await client.PostAsync("https://7777.bg/login/", content);
                var result = await response.Content.ReadAsStringAsync();
                var document = this.parser.Parse(result);
                var title = document.Title;
                if (title.ToLower().Contains("вход"))
                {
                    this.IsLoggedIn = false;
                    return false;
                }

                this.IsLoggedIn = true;
                return true;
            }
        }

        public async Task<string> Check(string barcode)
        {
            if (barcode.Length != 14)
            {
                return "Невалиден баркод";
            }

            var  handler = new HttpClientHandler();
            handler.CookieContainer = this.cookieContainer;
            using (var client = new HttpClient(handler))
            {
                var response = await client.GetAsync($"https://7777.bg/loto_games/check_talon/?talon_id={barcode}");
                var result = await response.Content.ReadAsStringAsync();
                var document = this.parser.Parse(result);
                var winning = document.GetElementsByClassName("winnig-text").FirstOrDefault();
                if (winning != null)
                {
                    return winning.TextContent;
                }

                return "Невалиден баркод";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}