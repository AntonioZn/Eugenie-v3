namespace Eugenie.Clients.Common.WebApiModels
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class Sell
    {
        public DateTime Date { get; set; }

        public float Total { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}