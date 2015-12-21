namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IServersManager
    {
        event EventHandler<ServerTestingFinishedEventArgs> ServerTestingFinished;

        Task<IDictionary<ServerInformation, HttpClient>> TestServers(IEnumerable<ServerInformation> servers);
    }
}