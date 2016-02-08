namespace Eugenie.Clients.AdminPanel.Contracts
{
    using System;

    using Common.Models;
    using Common.WebApiModels;

    using Models;

    public interface IServerManager
    {
        event EventHandler SelectedServerChanged;

        ServerInformation SelectedServer { get; }

        void SetSelectedServer(string name);

        event EventHandler ServerTestingFinished;

        Cache Cache { get; }

        void AddOrUpdate(string serverName, AddProductModel model);

        void Initialize();
        
        event EventHandler ProductsCacheChanged;

        void AddProductToCache(Product product);
    }
}