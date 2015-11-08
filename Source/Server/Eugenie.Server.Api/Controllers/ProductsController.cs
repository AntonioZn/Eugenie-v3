namespace Eugenie.Server.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using Data;
    using Data.Models;

    using Eugenie.Services.Data;

    using Models.Products;

    using Services.Data.Contracts;

    //[Authorize]
    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService = new ProductsService(new EfGenericRepository<Product>(new EugenieDbContext()), new EfGenericRepository<Barcode>(new EugenieDbContext()));
        
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
            var product = this.productsService.FindById(id).FirstOrDefault();

            if (product == null)
            {
                return this.NotFound();
            }

            this.productsService.Delete(product);
            return this.Ok();
        }

        /// <summary>
        /// Adds a new product
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Add(ProductRequestModel model)
        {
            if (model.Measure == 0)
            {
                model.Measure = MeasureType.бр;
            }

            this.productsService.Add(model.Name, model.BuyingPrice, model.SellingPrice, model.Measure, model.Quantity, model.Barcode, model.ExpirationDate);
            return this.Ok();
        }
    }
}