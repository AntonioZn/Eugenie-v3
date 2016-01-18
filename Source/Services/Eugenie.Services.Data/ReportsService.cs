namespace Eugenie.Services.Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    public class ReportsService : IReportsService
    {
        private readonly IRepository<Report> reportsRepository;

        public ReportsService(IRepository<Report> reportsRepository)
        {
            this.reportsRepository = reportsRepository;
        }

        public void AddStockPrice(decimal total)
        {
            var report = this.GetTodaysReport();

            report.StockPrice += total;

            this.reportsRepository.SaveChanges();
        }

        public IQueryable<Report> GetReports()
        {
            return this.reportsRepository.All().Include("Wastes").Include("Sells").Include("Shipments");
        }

        public Report GetTodaysReport()
        {
            var report = this.reportsRepository.All().FirstOrDefault(x => x.Date == DateTime.Today);

            if (report == null)
            {
                report = new Report();
                this.reportsRepository.Add(report);
            }

            return report;
        }
    }
}