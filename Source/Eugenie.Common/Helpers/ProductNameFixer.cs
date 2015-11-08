namespace Eugenie.Common.Helpers
{
    using System.Text.RegularExpressions;

    public static class ProductNameFixer
    {
        public static string Fix(string badName)
        {
            var goodName = badName
                .Trim()
                .ToLower()
                .Replace(" бр.", "бр")
                .Replace(" кг.", "кг")
                .Replace(" мл.", "мл")
                .Replace(" гр.", "г")
                .Replace(" г.", "г")
                .Replace(" л.", "л")
                .Replace(" м.", "м")
                .Replace("бр.", "бр")
                .Replace("кг.", "кг")
                .Replace("мл.", "мл")
                .Replace("гр.", "г")
                .Replace("г.", "г")
                .Replace("л.", "л")
                .Replace("м.", "м");

            goodName = Regex.Replace(goodName, @"\s+", " ");

            return goodName;
        }
    }
}