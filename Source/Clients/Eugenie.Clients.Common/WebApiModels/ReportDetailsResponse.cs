namespace Eugenie.Clients.Common.WebApiModels
{
    using System;
    using System.Collections.Generic;

    public class ReportDetailsResponse
    {
        public DateTime Date { get; set; }

        public IEnumerable<Waste> Waste { get; set; }

        public IEnumerable<Sell> Sells { get; set; }

        public IEnumerable<Shipment> Shipments { get; set; }
    }
}