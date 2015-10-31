using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Eugenie.Data;
using Eugenie.Data.Migrations;
using Eugenie.Data.Models;

namespace ProjectForTestingPurposes
{
    class Program
    {
        static void Main(string[] args)
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