namespace Eugenie.Clients.Common.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Models;

    public static class ExistingBarcodeChecker
    {
        public static Product Check(string barcode, Product mainProduct, IEnumerable<Product> products)
        {
            if (mainProduct.Barcodes.Any(x => x.Value == barcode))
            {
                return mainProduct;
            }

            var existingProduct = products.FirstOrDefault(x => x.Barcodes.Any(y => y.Value == barcode));
            return existingProduct;
        }
    }
}