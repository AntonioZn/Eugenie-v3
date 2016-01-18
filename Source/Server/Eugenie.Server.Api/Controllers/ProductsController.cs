﻿namespace Eugenie.Server.Api.Controllers
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
        [Route("count")]
        public IHttpActionResult GetCount()
        {
            return this.Ok(this.productsService.Count());
        }
        
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            var result = this.productsService.All().ToList();
            return this.Ok(result);
        }

        [HttpGet]
        public IHttpActionResult FindByName(string id)
        {
            return this.Ok(this.productsService.FindByName(id));
        }
        
        [HttpGet]
        public IHttpActionResult FindByQuantity(decimal quantity)
        {
            var products = this.productsService.FindByQuantity(quantity).ToList();

            return this.Ok(products);
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
                return this.Ok(this.productsService.AddOrUpdate(model.Name, model.OldName, model.BuyingPrice, model.SellingPrice, 
                    model.Measure, model.QuantityToAdd, model.Barcodes ?? Enumerable.Empty<Barcode>().ToList(), model.ExpirationDates ?? Enumerable.Empty<ExpirationDate>().ToList()));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}