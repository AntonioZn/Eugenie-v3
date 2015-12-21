namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;

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

        /// <summary>
        ///     Gets the count of all products so that they can be paged accordingly.
        /// </summary>
        /// <returns>Returns the count of all products in the store</returns>
        [HttpGet]
        public IHttpActionResult GetCount()
        {
            return this.Ok(this.productsService.Count());
        }

        /// <summary>
        ///     Gets one page of products
        /// </summary>
        /// <param name="page">Number of the page</param>
        /// <param name="pageSize">The size of each page</param>
        /// <returns>Returns one page of products</returns>
        [HttpGet]
        public IHttpActionResult GetByPage(int page, int pageSize)
        {
            return this.Ok(this.productsService.All(page, pageSize).Select(x => new
            {
                x.Id,
                x.Name,
                x.BuyingPrice,
                x.Barcodes,
                x.Measure
            }).ToList());
        }

        /// <summary>
        ///     Gets a products by it's name
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns>Returns a product</returns>
        [HttpGet]
        public IHttpActionResult GetByName(string name)
        {
            return this.Ok(this.productsService.FindByName(name));
        }

        /// <summary>
        ///     Gets all products that have quantity lower than the one specified in the query string
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <returns>Returns a list of products with low quantity</returns>
        [HttpGet]
        public IHttpActionResult GetByQuantity(decimal quantity)
        {
            var products = this.productsService.FindByQuantity(quantity).ToList();

            return this.Ok(products);
        }

        /// <summary>
        ///     Finds and deletes a product by its name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public IHttpActionResult AddOrUpdateProduct(AddProductModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                return this.Ok(this.productsService.AddOrUpdate(model.Name, model.OldName, model.BuyingPrice, model.SellingPrice, model.Measure, model.Quantity, model.Barcodes.Select(x => x.Value), model.ExpirationDates.Select(x => x.Date)));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}