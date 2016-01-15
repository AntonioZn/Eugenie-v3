namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.ObjectModel;

    using Common.Models;

    public interface IServerStorage
    {
        ObservableCollection<ServerInformation> Servers { get; set; }

        void AddServer(ServerInformation server);

        void DeleteServer(ServerInformation server);
    }
}