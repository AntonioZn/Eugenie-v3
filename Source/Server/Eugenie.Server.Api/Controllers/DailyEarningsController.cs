namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using Services.Data.Contracts;

    [Authorize(Roles = "Admin")]
    public class DailyEarningsController : ApiController
    {
        private readonly IDailyEarningsService dailyEarningsService;

        public DailyEarningsController(IDailyEarningsService dailyEarningsService)
        {
            this.dailyEarningsService = dailyEarningsService;
        }

        public IHttpActionResult Get()
        {
            var dailyEarnings = this.dailyEarningsService.GetEarnings().Select(x => new
            {
                Date = x.Date,
                Earning = x.Sells.Sum(y => (decimal?)y.Total) ?? 0,
                Waste = x.Waste.Sum(y => (decimal?)y.Total) ?? 0,
                StockPrice = x.StockPrice
            }).ToList();

            return this.Ok(dailyEarnings);
        }

        [HttpGet]
        public IHttpActionResult GetForDate(DateTime date)
        {
            var dailyEarning = this.dailyEarningsService.GetEarnings().FirstOrDefault(x => x.Date == date);
            if (dailyEarning != null)
            {
                return this.Ok(dailyEarning);
            }
            else
            {
                return this.NotFound();
            }
        }
    }
}