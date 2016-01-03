namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;

    using Common.Models;

    public interface IServerStorage
    {
        event EventHandler<ServerAddedEventArgs> ServerAdded;

        event EventHandler<ServerDeletedEventArgs> ServerDeleted;

        ICollection<ServerInformation> Servers { get; set; }

        void AddServer(ServerInformation server);

        void DeleteServer(ServerInformation server);
    }
}