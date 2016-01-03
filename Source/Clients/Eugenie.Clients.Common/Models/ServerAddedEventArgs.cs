namespace Eugenie.Clients.Common.Models
{
    using System;

    public class ServerAddedEventArgs : EventArgs
    {
        public ServerAddedEventArgs(ServerInformation server)
        {
            this.Server = server;
        }

        public ServerInformation Server { get; set; }
    }
}