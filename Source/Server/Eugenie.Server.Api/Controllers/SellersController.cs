namespace Eugenie.Server.Api.Controllers
{
    using System.Linq;
    using System.Web.Http;

    using Data;
    using Data.Models;

    using Microsoft.AspNet.Identity.EntityFramework;

    [Authorize(Roles = "Admin")]
    public class SellersController : ApiController
    {
        private readonly IRepository<IdentityRole> rolesRepository;
        private readonly IRepository<User> sellersRepository;

        public SellersController(IRepository<User> sellersRepository, IRepository<IdentityRole> rolesRepository)
        {
            this.rolesRepository = rolesRepository;
            this.sellersRepository = sellersRepository;
        }

        public IHttpActionResult Get()
        {
            var sellerRoleId = this.rolesRepository.All().FirstOrDefault(x => x.Name == "Seller").Id;

            return this.Ok(this.sellersRepository.All().Where(x => x.Roles.Any(r => r.RoleId == sellerRoleId)).Select(x => new
            {
                x.UserName,
                Fullname = x.FirstName + " " + x.LastName
            }));
        }
    }
}