namespace Eugenie.Data.Models
{
    using System;

    public class DailyEarning
    {
        public int Id { get; set; }

        public decimal Earnings { get; set; }

        public decimal StockPrice { get; set; }

        public decimal Waste { get; set; }

        public DateTime Date { get; set; }
    }
}