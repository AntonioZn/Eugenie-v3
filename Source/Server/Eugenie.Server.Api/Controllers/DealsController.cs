namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Web.Http;

    using Data;
    using Data.Models;

    using Models.Products;

    using Services.Data;
    using Services.Data.Contracts;

    [Authorize]
    [RoutePrefix("api/deals")]
    public class DealsController : ApiController
    {
        private static EugenieDbContext context = new EugenieDbContext();
        private readonly IDealsService dealsService = new DealsService(new EfGenericRepository<Product>(context), new EfGenericRepository<DailyEarning>(context), new EfGenericRepository<User>(context));

        [HttpPut]
        [Route("sell")]
        public IHttpActionResult Sell(SellProductsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.dealsService.Sell(model.SellerId, model.Products);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        [HttpPut]
        [Route("waste")]
        public IHttpActionResult Waste(SellProductsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.dealsService.Waste(model.SellerId, model.Products);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }
    }
}