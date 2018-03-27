namespace Eugenie.Clients.Seller.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Common.Models;

    public class FiscalPrinterHandler
    {
        private const string ReceiptFilename = "receipt.txt";

        public FiscalPrinterHandler()
        {
            Directory.CreateDirectory(SettingsManager.Get().ReceiptPath);
        }

        public void ExportReceipt(IEnumerable<Product> input)
        {
            var products = new List<Product>(input);
            this.PrepareProducts(products);

            var receipt = new StringBuilder();

            foreach (var product in products)
            {
                var name = product.Name;
                var price = $"{product.SellingPrice:0.00}".Replace(',', '.');
                var quantity = $"{product.Quantity:0.000}".Replace(',', '.');

                receipt.AppendLine($"S,1,______,_,__;{name};{price};{quantity};1;1;2;0;0;");
            }

            if (receipt.Length != 0)
            {
                receipt.AppendLine("T,1,______,_,__;");
                receipt.AppendLine("O,1,______, ,__;");
            }

            var receiptArray = receipt.ToString().Trim().Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            File.WriteAllLines(Path.Combine(SettingsManager.Get().ReceiptPath, ReceiptFilename), receiptArray, Encoding.Default);
        }

        private void PrepareProducts(List<Product> products)
        {
            products.RemoveAll(x => x.Name.Contains("*"));

            var coupon = products.FirstOrDefault(x => x.Name.Contains("$"));
            if (coupon != null)
            {
                for (var i = 0; i < coupon.Quantity; i++)
                {
                    var bread = products.First(x => x.Name.Contains("хляб") && !x.Name.Contains("$") && x.Quantity > 0 && x.SellingPrice > coupon.SellingPrice * -1);
                    bread.Quantity--;

                    var breadWithCouponName = bread.Name + " с купон";

                    var breadWithCoupon = products.FirstOrDefault(x => x.Name == breadWithCouponName);

                    if (breadWithCoupon != null)
                    {
                        breadWithCoupon.Quantity++;
                    }
                    else
                    {
                        breadWithCoupon = new Product { Name = breadWithCouponName, SellingPrice = bread.SellingPrice - coupon.SellingPrice * -1, Quantity = 1 };
                        products.Add(breadWithCoupon);
                    }
                }

                products.RemoveAll(x => x.Quantity == 0);
                products.RemoveAll(x => x.Name.Contains("$"));
            }
        }
    }
}