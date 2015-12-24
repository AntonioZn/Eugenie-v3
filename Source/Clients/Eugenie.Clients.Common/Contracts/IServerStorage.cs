namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;

    using Common.Models;

    public interface IServerStorage
    {
        ICollection<ServerInformation> Servers { get; set; }

        void AddServer(ServerInformation server);

        void DeleteServer(ServerInformation server);
    }
}