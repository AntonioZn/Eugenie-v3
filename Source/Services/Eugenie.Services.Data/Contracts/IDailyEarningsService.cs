namespace Eugenie.Services.Data.Contracts
{
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IDailyEarningsService
    {
        IQueryable<DailyEarning> GetEarnings();

        void AddWaste(decimal total);

        void AddSell(decimal total);

        void AddStockPrice(decimal total);
    }
}