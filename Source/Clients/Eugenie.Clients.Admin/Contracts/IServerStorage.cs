namespace Eugenie.Clients.Admin.Contracts
{
    using System.Collections.ObjectModel;

    using Common.Models;

    using Store = Models.Store;

    public interface IServerStorage
    {
        ObservableCollection<Store> Servers { get; }
    }
}