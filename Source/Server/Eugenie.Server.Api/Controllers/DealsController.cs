namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Web.Http;

    using Microsoft.AspNet.Identity;

    using Models.Products;

    using Services.Data.Contracts;

    [Authorize]
    [RoutePrefix("api/deals")]
    public class DealsController : ApiController
    {
        private readonly IDealsService dealsService;

        public DealsController(IDealsService dealsService)
        {
            this.dealsService = dealsService;
        }

        [HttpGet]
        [Route("sells")]
        public IHttpActionResult GetSells(string sellerId, string startDate, string endDate)
        {
            try
            {
                return this.Ok(this.dealsService.GetSells(sellerId, startDate, endDate));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("waste")]
        public IHttpActionResult GetWaste(string sellerId, string startDate, string endDate)
        {
            try
            {
                return this.Ok(this.dealsService.GetWaste(sellerId, startDate, endDate));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("sells")]
        public IHttpActionResult Sell(SellProductsModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.dealsService.Sell(this.User.Identity.GetUserId(), model.Products);
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
                this.dealsService.Waste(this.User.Identity.GetUserId(), model.Products);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }
    }
}