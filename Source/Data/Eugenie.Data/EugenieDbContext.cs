namespace Eugenie.Data
{
    using System.Data.Entity;

    using Microsoft.AspNet.Identity.EntityFramework;

    using Models;

    public class EugenieDbContext : IdentityDbContext<User>
    {
        public EugenieDbContext()
            : base("Eugenie", false)
        {
        }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Barcode> Barcodes { get; set; }

        public IDbSet<ExpirationDate> ExpirationDates { get; set; }

        public IDbSet<Sell> Sells { get; set; }

        public IDbSet<Waste> Waste { get; set; }

        public IDbSet<DailyEarning> DailyEarnings { get; set; }

        public IDbSet<MissingProduct> MissingProducts { get; set; }

        public static EugenieDbContext Create()
        {
            return new EugenieDbContext();
        }
    }
}