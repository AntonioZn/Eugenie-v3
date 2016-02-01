namespace Eugenie.Clients.Common.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Models;

    public class TotalSellingPriceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var product = value as Product;
            return product.Quantity * product.SellingPrice;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}