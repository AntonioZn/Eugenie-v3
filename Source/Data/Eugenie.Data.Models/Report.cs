namespace Eugenie.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public class Report
    {
        private ICollection<Sell> sells;
        private ICollection<Waste> waste;
        private ICollection<Shipment> shipments;

        public Report()
        {
            this.Date = DateTime.Today;
            this.sells = new HashSet<Sell>();
            this.waste = new HashSet<Waste>();
            this.shipments = new HashSet<Shipment>();
        }

        [JsonIgnore]
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public decimal StockPrice { get; set; }

        public virtual ICollection<Sell> Sells
        {
            get
            {
                return this.sells;
            }

            set
            {
                this.sells = value;
            }
        }

        public virtual ICollection<Waste> Waste
        {
            get
            {
                return this.waste;
            }

            set
            {
                this.waste = value;
            }
        }

        public virtual ICollection<Shipment> Shipments
        {
            get
            {
                return this.shipments;
            }

            set
            {
                this.shipments = value;
            }
        }
    }
}