namespace Eugenie.Server.Api.Controllers
{
    using System;
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
            var reports = this.reportsService.GetReports().Select(x => new
            {
                x.Date,
                Earning = x.Sells.Sum(y => (decimal?)y.Total) ?? 0,
                Waste = x.Waste.Sum(y => (decimal?)y.Total) ?? 0,
                x.StockPrice
            }).OrderByDescending(x => x.Date);

            return this.Ok(reports);
        }

        public IHttpActionResult Get(DateTime date)
        {
            var reports = this.reportsService.GetReports().Select(r => new
            {
                r.Date,
                Waste = r.Waste.Select(w => new
                {
                    w.Date,
                    w.Total,
                    Products = w.Products.Select(pr => new
                    {
                        pr.Product.Name,
                        pr.Quantity
                    })
                }).OrderByDescending(x => x.Date),
                Sells = r.Sells.Select(s => new
                {
                    s.Date,
                    s.Total,
                    Products = s.Products.Select(pr => new
                    {
                        pr.Product.Name,
                        pr.Quantity
                    })
                }).OrderByDescending(x => x.Date),
                Shipments = r.Shipments.Select(sh => new
                                                     {
                                                         sh.Product.Name,
                                                         sh.Product.SellingPrice,
                                                         sh.Quantity
                                                     })
            }).FirstOrDefault(x => x.Date == date);

            return this.Ok(reports);
        }
    }
}