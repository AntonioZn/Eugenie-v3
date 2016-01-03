namespace Eugenie.Clients.Common.Models
{
    using System;

    public class ServerDeletedEventArgs : EventArgs
    {
        public ServerDeletedEventArgs(ServerInformation server)
        {
            this.Server = server;
        }

        public ServerInformation Server { get; set; }
    }
}