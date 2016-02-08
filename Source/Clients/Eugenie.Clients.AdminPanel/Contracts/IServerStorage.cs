namespace Eugenie.Clients.AdminPanel.Contracts
{
    using System.Collections.ObjectModel;

    using Common.Models;

    public interface IServerStorage
    {
        ObservableCollection<ServerInformation> Servers { get; }
    }
}