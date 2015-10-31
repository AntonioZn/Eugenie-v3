namespace Eugenie.Data
{
    using System.Data.Entity;
    using Eugenie.Data.Models;

    public class EugenieDbContext : DbContext
    {
        public EugenieDbContext() : base ("Eugenie")
        {
        }

        public IDbSet<Product> Products { get; set; }
        
        public IDbSet<Barcode> Barcodes { get; set; }

        public IDbSet<ExpirationDate> ExpirationDates { get; set; }

        public IDbSet<Sell> Sells { get; set; }

        public IDbSet<Waste> Waste { get; set; }

        public IDbSet<DailyEarning> DailyEarnings { get; set; }

        public IDbSet<User> Users { get; set; }

        public IDbSet<MissingProduct> MissingProducts { get; set; }

        public static EugenieDbContext Create()
        {
            return new EugenieDbContext();
        }
    }
}