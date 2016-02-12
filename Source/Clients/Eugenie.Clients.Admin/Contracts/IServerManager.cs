namespace Eugenie.Clients.Admin.Contracts
{
    using System;
    using System.Collections.Generic;

    using Common.Models;

    using Models;

    using ViewModels;

    public interface IServerManager
    {
        ServerInformation SelectedServer { get; }

        Cache Cache { get; }

        event EventHandler SelectedServerChanged;

        void SetSelectedServer(string name);

        event EventHandler ProductsCacheChanged;

        void AddOrUpdate(IDictionary<ServerInformation, ProductViewModel> productInAllServers, ProductViewModel mainProductViewModel);

        event EventHandler ServerTestingFinished;

        void Initialize();

        void Delete(string productName);
    }
}