namespace Eugenie.Clients.Common.Models
{
    using System;

    using Newtonsoft.Json;

    public class ServerInformation : IComparable<ServerInformation>
    {
        public ServerInformation(string username, string password, string name, string address)
        {
            this.Username = username;
            this.Password = password;
            this.Name = name;
            this.Uri = new Uri(address);
        }
        
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        [JsonIgnore]
        public string AuthToken { get; set; }

        [JsonIgnore]
        public TimeSpan Ping { get; set; }

        public int CompareTo(ServerInformation other)
        {
            return this.Ping.CompareTo(other.Ping);
        }
    }
}