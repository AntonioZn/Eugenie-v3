namespace Eugenie.Clients.Common.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Models;

    using WebApiModels;

    public interface IWebApiClient
    {
        Task<ICollection<Product>> GetProductsAsync(HttpClient client);

        Task<HttpStatusCode> AddOrUpdateAsync(HttpClient client, AddProductModel model);

        Task<IEnumerable<MissingProduct>> GetMissingProductsAsync(HttpClient client);

        Task<ICollection<Report>> GetReportsAsync(HttpClient client);

        Task<ReportDetailsResponse> GetReportDetailsAsync(HttpClient client, DateTime date);

        Task<IEnumerable<Seller>> GetSellersAsync(HttpClient client);
    }
}