namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Linq;
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
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetDeals(string username, DateTime start, DateTime end)
        {
            var sells = this.dealsService.GetSells(username, start, end.AddDays(1)).Select(x => new
            {
                Date = x.Date,
                Total = x.Total,
                Products = x.Products.Select(pr => new
                {
                    Name = pr.Product.Name,
                    Quantity = pr.Quantity,
                })
            });

            var waste = this.dealsService.GetWaste(username, start, end.AddDays(1)).Select(x => new
            {
                Date = x.Date,
                Total = x.Total,
                Products = x.Products.Select(pr => new
                {
                    Name = pr.Product.Name,
                    Quantity = pr.Quantity,
                })
            });

            var result = new
                         {
                             Sells = sells,
                             Waste = waste
                         };

            return this.Ok(result);
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