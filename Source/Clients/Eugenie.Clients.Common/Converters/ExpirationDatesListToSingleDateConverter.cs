namespace Eugenie.Clients.Common.Converters
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    using Models;

    public class ExpirationDatesListToSingleDateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dates = ((IList) value).Cast<ExpirationDate>();

            return dates.OrderBy(x => x.Date).FirstOrDefault().Date;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}