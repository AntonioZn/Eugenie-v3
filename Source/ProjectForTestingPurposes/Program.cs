namespace ProjectForTestingPurposes
{
    using System.Data.Entity;

    using Eugenie.Data;
    using Eugenie.Data.Migrations;
    using Eugenie.Data.Models;

    internal class Program
    {
        private static void Main(string[] args)
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<EugenieDbContext, Configuration>());

            var context = new EugenieDbContext();

            var product = new Product();
            product.Name = "testtesttest";
            product.Measure = MeasureType.кг;

            context.Products.Add(product);
            context.SaveChanges();
        }
    }
}