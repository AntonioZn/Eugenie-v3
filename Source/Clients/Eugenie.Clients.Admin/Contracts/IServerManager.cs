namespace Eugenie.Clients.Admin.Contracts
{
    using System;

    using Common.Models;
    using Common.WebApiModels;

    using Models;

    public interface IServerManager
    {
        ServerInformation SelectedServer { get; }

        Cache Cache { get; }

        event EventHandler SelectedServerChanged;

        void SetSelectedServer(string name);

        event EventHandler ServerTestingFinished;

        void AddOrUpdate(string serverName, AddProductModel model);

        void Initialize();

        event EventHandler ProductsCacheChanged;

        void AddProductToCache(Product product);

        void Delete(string productName);
    }
}