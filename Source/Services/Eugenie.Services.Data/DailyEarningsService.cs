namespace Eugenie.Services.Data
{
    using System;
    using System.Linq;

    using Contracts;

    using Eugenie.Data;
    using Eugenie.Data.Models;

    public class DailyEarningsService : IDailyEarningsService
    {
        private readonly IRepository<DailyEarning> dailyEarningsRepository;

        public DailyEarningsService(IRepository<DailyEarning> dailyEarningsRepository)
        {
            this.dailyEarningsRepository = dailyEarningsRepository;
        }

        public void AddStockPrice(decimal total)
        {
            var dailyEarning = this.GetDailyEarning();

            dailyEarning.StockPrice += total;

            this.dailyEarningsRepository.SaveChanges();
        }

        public IQueryable<DailyEarning> GetEarnings()
        {
            return this.dailyEarningsRepository.All();
        }

        public DailyEarning GetDailyEarning()
        {
            var dailyEarning = this.dailyEarningsRepository.All().FirstOrDefault(x => x.Date == DateTime.Today);

            if (dailyEarning == null)
            {
                dailyEarning = new DailyEarning();
                this.dailyEarningsRepository.Add(dailyEarning);
            }

            return dailyEarning;
        }
    }
}