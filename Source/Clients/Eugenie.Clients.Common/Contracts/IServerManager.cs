namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Threading.Tasks;

    using Models;

    using WebApiModels;

    public interface IServerManager
    {
        event EventHandler SelectedServerChanged;

        ServerInformation SelectedServer { get; }

        void SetSelectedServer(string name);

        event EventHandler ServerTestingFinished;

        Cache Cache { get; }

        void AddOrUpdate(ServerInformation server, AddProductModel model);

        void Initialize();
        
        event EventHandler ProductsCacheChanged;

        void AddProductToCache(Product product);
    }
}