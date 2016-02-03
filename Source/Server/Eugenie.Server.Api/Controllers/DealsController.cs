namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;

    using Data.Models.DummyModels;

    using Microsoft.AspNet.Identity;

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
                x.Date,
                x.Total,
                Products = x.Products.Select(pr => new
                {
                    pr.Product.Name,
                    pr.Quantity,
                })
            }).OrderByDescending(x => x.Date);

            var waste = this.dealsService.GetWaste(username, start, end.AddDays(1)).Select(x => new
            {
                x.Date,
                x.Total,
                Products = x.Products.Select(pr => new
                {
                    pr.Product.Name,
                    pr.Quantity,
                })
            }).OrderByDescending(x => x.Date);

            var result = new
                         {
                             Sells = sells,
                             Waste = waste
                         };

            return this.Ok(result);
        }

        [HttpPut]
        [Route("sell")]
        public IHttpActionResult Sell(IEnumerable<IdQuantityPair> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.dealsService.Sell(this.User.Identity.GetUserId(), model);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }

        [HttpPut]
        [Route("waste")]
        public IHttpActionResult Waste(IEnumerable<IdQuantityPair> model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.dealsService.Waste(this.User.Identity.GetUserId(), model);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }

            return this.Ok();
        }
    }
}