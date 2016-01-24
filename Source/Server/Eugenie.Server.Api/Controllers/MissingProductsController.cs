namespace Eugenie.Server.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using Services.Data.Contracts;

    [Authorize(Roles = "Admin")]
    public class MissingProductsController : ApiController
    {
        private readonly IMissingProductsService missingProductsService;

        public MissingProductsController(IMissingProductsService missingProductsService)
        {
            this.missingProductsService = missingProductsService;
        }

        [HttpGet]
        public IHttpActionResult GetMissingProducts()
        {
            return this.Ok(this.missingProductsService.GetMissingProducts().Select(x => new
                                                                                        {
                                                                                            Name = x.Name,
                                                                                            Barcode = x.Barcode,
                                                                                            Date = x.Date
                                                                                        }).ToList());
        }
    }
}