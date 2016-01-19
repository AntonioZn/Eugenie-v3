namespace Eugenie.Clients.Common.WebApiModels
{
    using System;
    using System.Collections.Generic;

    using Models;

    public class Waste
    {
        private DateTime date;

        public DateTime Date
        {
            get
            {
                return this.date;
            }

            set
            {
                this.date = DateTime.SpecifyKind(DateTime.Parse(value.ToString()), DateTimeKind.Utc).ToLocalTime();
            }
        }

        public decimal Total { get; set; }

        public IEnumerable<Product> Products { get; set; }
    }
}