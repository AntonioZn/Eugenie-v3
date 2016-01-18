namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Http;

    using Services.Data.Contracts;

    [Authorize(Roles = "Admin")]
    public class ReportsController : ApiController
    {
        private readonly IReportsService reportsService;

        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }

        public IHttpActionResult Get()
        {
            var reports = this.reportsService.GetReports().Include("Wastes").Include("Sells").Include("Shipments").Select(x => new
            {
                Date = x.Date,
                Earning = x.Sells.Sum(y => (decimal?)y.Total) ?? 0,
                Waste = x.Waste.Sum(y => (decimal?)y.Total) ?? 0,
                StockPrice = x.StockPrice
            }).ToList();

            return this.Ok(reports);
        }
        
        public IHttpActionResult Get(DateTime date)
        {
            var reports = this.reportsService.GetReports().Include("Waste").Include("Sells").Include("Shipments").FirstOrDefault(x => x.Date == date);
            if (reports != null)
            {
                return this.Ok(reports);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}