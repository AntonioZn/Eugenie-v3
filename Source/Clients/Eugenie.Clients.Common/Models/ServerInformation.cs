namespace Eugenie.Clients.Common.Models
{
    using System;

    public class ServerInformation
    {
        public ServerInformation(string name, string address, string authToken)
        {
            this.Name = name;
            this.Uri = new Uri(address);
            this.AuthToken = authToken;
        }

        public string Name { get; set; }

        public Uri Uri { get; set; }

        public string AuthToken { get; set; }
    }
}