namespace Eugenie.Server.Api.Controllers
{
    using System.Web.Http;

    using Services.Data.Contracts;

    [Authorize]
    public class DailyEarningsController : ApiController
    {
        private readonly IDailyEarningsService dailyEarningsService;

        public DailyEarningsController(IDailyEarningsService dailyEarningsService)
        {
            this.dailyEarningsService = dailyEarningsService;
        }

        public IHttpActionResult Get()
        {
            return this.Ok(this.dailyEarningsService.GetEarnings());
        }
    }
}