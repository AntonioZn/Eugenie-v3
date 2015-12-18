namespace Eugenie.Clients.Common.Contracts
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IServersManager
    {
        IDictionary<ServerInformation, HttpClient> ActiveServers { get; }

        HttpClient FastestServer { get; }

        Task<IDictionary<ServerInformation, HttpClient>> TestServers(IEnumerable<ServerInformation> servers);
    }
}