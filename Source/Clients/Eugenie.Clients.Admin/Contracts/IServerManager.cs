namespace Eugenie.Clients.Admin.Contracts
{
    using System;
    using System.Collections.Generic;

    using Common.Models;

    using ViewModels;

    using Store = Models.Store;

    public interface IServerManager
    {
        event EventHandler SelectedServerChanged;

        event EventHandler ProductsCacheChanged;

        event EventHandler ServerTestingFinished;

        Store SelectedServer { get; }

        void SetSelectedServer(string name);

        void AddOrUpdate(IDictionary<Store, ProductViewModel> productInAllServers, ProductViewModel mainProductViewModel);

        void Initialize();

        void Delete(string productName);
    }
}