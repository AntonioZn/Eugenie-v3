namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

    using Data.Models;

    using Models.Products;

    using Services.Data.Contracts;

    [Authorize]
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService;

        public ProductsController(IProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("count")]
        public IHttpActionResult GetCount()
        {
            return this.Ok(this.productsService.Count());
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetAll()
        {
            return this.Ok(this.productsService.All());
        }

        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            return this.Ok(this.productsService.GetById(id).Select(x => new
            {
                x.Id,
                x.Name,
                x.Measure,
                x.SellingPrice
            }).FirstOrDefault());
        }

        [HttpGet]
        public IHttpActionResult GetByName(string name)
        {
            return this.Ok(this.productsService.GetByName(name).Select(x => new
            {
                x.Id,
                x.Name,
                x.Measure,
                x.SellingPrice
            }));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetByQuantity(decimal quantity)
        {
            return this.Ok(this.productsService.GetByQuantity(quantity).Select(x => new
                                                                                    {
                                                                                        x.Name,
                                                                                        x.Quantity
                                                                                    }));
        }

        [HttpGet]
        public IHttpActionResult GetByBarcode(string barcode)
        {
            return this.Ok(this.productsService.GetByBarcode(barcode).Select(x => new
            {
                x.Id,
                x.Name,
                x.Measure,
                x.SellingPrice
            }).FirstOrDefault());
        }

        [HttpGet]
        public IHttpActionResult GetByExpirationDate(int days)
        {
            return this.Ok(this.productsService.GetByExpirationDate(days).Select(x => new
                                                                                      {
                                                                                          x.Name,
                                                                                          x.ExpirationDates
                                                                                      }));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult Delete(string id)
        {
            try
            {
                this.productsService.Delete(id);
                return this.Ok();
            }
            catch
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult AddOrUpdate(AddProductModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                this.productsService.AddOrUpdate(model.Name, model.OldName, model.BuyingPrice, model.SellingPrice,
                    model.Measure, model.QuantityToAdd, model.Barcodes ?? Enumerable.Empty<Barcode>().ToList(), model.ExpirationDates ?? Enumerable.Empty<ExpirationDate>().ToList());
                return this.Ok();
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}