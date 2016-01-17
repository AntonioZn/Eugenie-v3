﻿namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.ObjectModel;

    using Common.Models;

    public interface IServerStorage
    {
        ObservableCollection<ServerInformation> Servers { get; }
    }
}