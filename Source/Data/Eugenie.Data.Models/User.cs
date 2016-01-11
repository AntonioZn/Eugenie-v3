namespace Eugenie.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Common.Constants;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    public class User : IdentityUser
    {
        private ICollection<Sell> sells;
        private ICollection<Waste> waste;

        public User()
        {
            this.sells = new HashSet<Sell>();
            this.waste = new HashSet<Waste>();
        }

        [MaxLength(ValidationConstants.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [MaxLength(ValidationConstants.LastNameMaxLength)]
        public string LastName { get; set; }

        public virtual ICollection<Sell> Sells
        {
            get
            {
                return this.sells;
            }
            set
            {
                this.sells = value;
            }
        }

        public virtual ICollection<Waste> Waste
        {
            get
            {
                return this.waste;
            }
            set
            {
                this.waste = value;
            }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }
}