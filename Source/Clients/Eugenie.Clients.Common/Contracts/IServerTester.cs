namespace Eugenie.Clients.Common.Contracts
{
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    public interface IServerTester
    {
        Task<HttpClient> TestServer(ServerInformation server);
    }
}