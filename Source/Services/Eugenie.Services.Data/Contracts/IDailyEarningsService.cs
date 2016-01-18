namespace Eugenie.Services.Data.Contracts
{
    using System.Linq;

    using Eugenie.Data.Models;

    public interface IDailyEarningsService
    {
        IQueryable<DailyEarning> GetEarnings();

        void AddStockPrice(decimal total);

        DailyEarning GetDailyEarning();
    }
}