namespace Eugenie.Services.Data.Contracts
{
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IReportsService
    {
        IQueryable<Report> GetReports();

        void AddStockPrice(decimal total);

        Report GetTodaysReport();

        void AddShipment(Product product, decimal quantity);
    }
}