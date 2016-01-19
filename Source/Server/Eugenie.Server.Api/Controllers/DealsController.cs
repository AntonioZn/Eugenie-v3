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
        [Route("sells")]
        public IHttpActionResult GetSells(string name, DateTime start, DateTime end)
        {
            try
            {
                var sells = this.dealsService.GetSells(name, start, end.AddDays(1)).Select(x => new
                {
                    Date = x.Date,
                    Total = x.Total,
                    Products = x.Products.Select(pr => new
                    {
                        Name = pr.Product.Name,
                        Quantity = pr.Quantity,
                    })
                });

                return this.Ok(sells);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("waste")]
        public IHttpActionResult GetWaste(string name, DateTime start, DateTime end)
        {
            try
            {
                var waste = this.dealsService.GetWaste(name, start, end.AddDays(1)).Select(x => new
                {
                    Date = x.Date,
                    Total = x.Total,
                    Products = x.Products.Select(pr => new
                    {
                        Name = pr.Product.Name,
                        Quantity = pr.Quantity,
                    })
                });

                return this.Ok(waste);
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