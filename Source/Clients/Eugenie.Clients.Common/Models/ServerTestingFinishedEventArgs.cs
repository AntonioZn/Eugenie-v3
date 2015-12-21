namespace Eugenie.Clients.Common.Models
{
    using System;
    using System.Collections.Generic;

    using System.Net.Http;

    public class ServerTestingFinishedEventArgs : EventArgs
    {
        public ServerTestingFinishedEventArgs(IDictionary<ServerInformation, HttpClient> activeServers)
        {
            this.ActiveServers = activeServers;
        }

        public IDictionary<ServerInformation, HttpClient> ActiveServers { get; set; }
    }
}