namespace Eugenie.Clients.Common.Models
{
    using System;

    using Newtonsoft.Json;

    public class ServerInformation
    {
        public ServerInformation(string username, string password, string name, string address)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Address = address;
            this.Uri = new Uri(address);
        }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        [JsonIgnore]
        public Uri Uri { get; set; }

        [JsonIgnore]
        public string AuthToken { get; set; }

        [JsonIgnore]
        public TimeSpan Ping { get; set; }
    }
}