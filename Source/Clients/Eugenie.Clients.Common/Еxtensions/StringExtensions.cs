namespace Eugenie.Clients.Common.Еxtensions
{
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string RemoveMultipleWhiteSpaces(this string argument)
        {
            return Regex.Replace(argument.TrimStart(), @"\s+", " ");
        }
    }
}