namespace Eugenie.Services.Data.Contracts
{
    using System.Collections.Generic;

    using Eugenie.Data.Models;
    using Eugenie.Data.Models.DummyModels;

    public interface IDealsService
    {
        void Sell(string sellerId, IEnumerable<IdQuantityPair> products);

        void Waste(string sellerId, IEnumerable<IdQuantityPair> products);

        IEnumerable<Sell> GetSells(string sellerId, string startDate, string endDate);

        IEnumerable<Waste> GetWaste(string sellerId, string startDate, string endDate);
    }
}