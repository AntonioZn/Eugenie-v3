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
    [RoutePrefix("api/products")]
    public class ProductsController : ApiController
    {
        private readonly IProductsService productsService = new ProductsService(new EfGenericRepository<Product>(new EugenieDbContext()), new EfGenericRepository<Barcode>(new EugenieDbContext()));

        [Route("count")]
        [HttpGet]
        public IHttpActionResult GetCount()
        {
            return this.Ok(this.productsService.Count());
        }
        
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