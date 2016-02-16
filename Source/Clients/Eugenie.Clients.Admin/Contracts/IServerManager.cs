namespace Eugenie.Clients.Admin.Contracts
{
    using System;
    using System.Collections.Generic;

    using Common.Models;

    using Models;

    using ViewModels;

    public interface IServerManager
    {
        event EventHandler SelectedServerChanged;

        event EventHandler ProductsCacheChanged;

        event EventHandler ServerTestingFinished;

        ServerInformation SelectedServer { get; }

        Cache Cache { get; }

        void SetSelectedServer(string name);

        void AddOrUpdate(IDictionary<ServerInformation, ProductViewModel> productInAllServers, ProductViewModel mainProductViewModel);

        void Initialize();

        void Delete(string productName);
    }
}