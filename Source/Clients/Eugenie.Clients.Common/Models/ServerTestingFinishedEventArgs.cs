namespace Eugenie.Clients.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class ServerTestingFinishedEventArgs : EventArgs
    {
        public ServerTestingFinishedEventArgs(IEnumerable<ServerInformation> activeServers)
        {
            this.ActiveServers = activeServers;
        }

        public IEnumerable<ServerInformation> ActiveServers { get; set; }
    }
}