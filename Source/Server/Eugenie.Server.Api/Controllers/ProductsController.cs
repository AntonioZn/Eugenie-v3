namespace Eugenie.Server.Api.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    
    using Data.Models;

    using Models.Products;

    using Services.Data.Contracts;

    //TODO: fix summaries
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
        /// Gets the count of all products so that they can be paged accordingly.
        /// </summary>
        /// <returns>Returns the count of all products in the store</returns>
        [HttpGet]
        public IHttpActionResult GetCount()
        {
            return this.Ok(this.productsService.Count());
        }
        
        /// <summary>
        /// Gets one page of products
        /// </summary>
        /// <param name="page">Number of the page</param>
        /// <param name="pageSize">The size of each page</param>
        /// <returns>Returns one page of products</returns>
        [HttpGet]
        public IHttpActionResult GetByPage(int page, int pageSize)
        {
            return this.Ok(this.productsService.All(page, pageSize).Select(x => new
                                                                                {
                                                                                    x.Name,
                                                                                    x.SellingPrice,
                                                                                    x.BuyingPrice,
                                                                                    x.Barcodes,
                                                                                    x.Measure
                                                                                }).ToList());
        }
        
        /// <summary>
        /// Gets a product by its Id if such exists
        /// </summary>
        /// <param name="id">An Id of a existing product</param>
        /// <returns>One product</returns>
        [HttpGet]
        public IHttpActionResult GetById(int id)
        {
            var product = this.productsService.FindById(id).FirstOrDefault();

            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok(product);
        }
        
        /// <summary>
        /// Gets a List of products that fullfil the search query and sorts them accordingly
        /// </summary>
        /// <param name="name">Product name</param>
        /// <returns>Returns a list of products</returns>
        [HttpGet]
        public IHttpActionResult GetByName(string name)
        {
            var products = this.productsService.FindByName(name).ToList();

            if (products.Count == 0)
            {
                return this.NotFound();
            }

            return this.Ok(products);
        }

        /// <summary>
        /// Gets a product by its barcode if such exists
        /// </summary>
        /// <param name="barcode">Barcode of an existing product</param>
        /// <returns>Returns a list of products</returns>
        [HttpGet]
        public IHttpActionResult GetByBarcode(string barcode)
        {
            var product = this.productsService.FindByBarcode(barcode).FirstOrDefault();

            if (product == null)
            {
                return this.NotFound();
            }

            return this.Ok(product);
        }

        /// <summary>
        /// Gets all products that have quantity lower than the one specified in the query string
        /// </summary>
        /// <param name="quantity">Quantity</param>
        /// <returns>Returns a list of products with low quantity</returns>
        [HttpGet]
        public IHttpActionResult GetByQuantity(decimal quantity)
        {
            var products = this.productsService.FindByQuantity(quantity).ToList();

            if (products.Count == 0)
            {
                return this.NotFound();
            }

            return this.Ok(products);
        }

        /// <summary>
        /// Finds and deletes a product by its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            this.productsService.Delete(id);
            return this.Ok();
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the new product</returns>
        [HttpPost]
        public IHttpActionResult Add(AddProductModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (model.Measure == 0)
            {
                model.Measure = MeasureType.бр;
            }

            try
            {
                return this.Ok(this.productsService.Add(model.Name, model.BuyingPrice, model.SellingPrice, model.Measure, model.Quantity, model.Barcode, model.ExpirationDate));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Adds a barcode to an existing product.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the updated product or error 409 if the barcode already exists in the database</returns>
        [HttpPost]
        [Route("addBarcode")]
        public IHttpActionResult AddBarcode(AddBarcodeModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                return this.Ok(this.productsService.AddBarcode(model.Id, model.Barcode));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        //TODO: check for default values
        /// <summary>
        /// Adds and expiration date to an existing product.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the updated product or error 409 if the product already has this expiration date</returns>
        [HttpPost]
        [Route("addExpirationDate")]
        public IHttpActionResult AddExpirationDate(AddExpirationDateModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                return this.Ok(this.productsService.AddExpirationDate(model.Id, model.Date));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Sets all product properties to the new ones except quantity, which is added to the existing
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Returns the updated product</returns>
        [HttpPut]
        public IHttpActionResult Update(UpdateProductModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            try
            {
                return this.Ok(this.productsService.Update(model.Id, model.Name, model.BuyingPrice, model.SellingPrice, model.Measure, model.Quantity));
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }
    }
}