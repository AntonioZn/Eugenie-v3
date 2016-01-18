namespace Eugenie.Clients.Common.WebApiModels
{
    using System;

    public class Report
    {
        public DateTime Date { get; set; }

        public decimal Earning { get; set; }

        public decimal Waste { get; set; }

        public decimal StockPrice { get; set; }
    }
}