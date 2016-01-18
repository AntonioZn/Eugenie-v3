namespace Eugenie.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class DailyEarning
    {
        private ICollection<Sell> sells;
        private ICollection<Waste> waste; 
         
        public DailyEarning()
        {
            this.Date = DateTime.Today;
            this.sells = new HashSet<Sell>();
            this.waste = new HashSet<Waste>();
        }

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
    }
}