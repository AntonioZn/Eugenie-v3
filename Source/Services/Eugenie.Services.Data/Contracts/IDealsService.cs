namespace Eugenie.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Eugenie.Data.Models;
    using Eugenie.Data.Models.DummyModels;

    public interface IDealsService
    {
        void Sell(string sellerId, IEnumerable<IdQuantityPair> products);

        void Waste(string sellerId, IEnumerable<IdQuantityPair> products);

        IQueryable<Sell> GetSells(string name, DateTime start, DateTime end);

        IQueryable<Waste> GetWaste(string name, DateTime start, DateTime end);
    }
}