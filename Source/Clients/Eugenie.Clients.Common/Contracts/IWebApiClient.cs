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

        Task<ReportDetailsResponse> GetDealsForSeller(HttpClient client, string username, DateTime start, DateTime end);

        Task<UserInfoResponse> GetUserInfo(HttpClient client);

        Task<IEnumerable<Product>> GetProductsByNameAsync(HttpClient client, string name);

        Task<Product> GetProductById(HttpClient client, int id);

        Task<Product> GetProductByBarcode(HttpClient client, string barcode);
    }
}