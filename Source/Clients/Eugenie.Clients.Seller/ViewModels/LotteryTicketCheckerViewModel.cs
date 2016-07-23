namespace Eugenie.Clients.Seller.ViewModels
{
    using Common.Contracts;

    using GalaSoft.MvvmLight;

    using Helpers;

    public class LotteryTicketCheckerViewModel : ViewModelBase, IBarcodeHandler
    {
        private readonly LotteryTicketChecker lotteryTicketChecker;
        private string winning;
        private bool isLoading;

        public LotteryTicketCheckerViewModel(LotteryTicketChecker lotteryTicketChecker)
        {
            this.lotteryTicketChecker = lotteryTicketChecker;
        }

        public string Winning
        {
            get
            {
                return this.winning;
            }
            set
            {
                this.Set(() => this.Winning, ref this.winning, value);
            }
        }

        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }
            set
            {
                this.Set(() => this.IsLoading, ref this.isLoading, value);
            }
        }

        public async void HandleBarcode(string barcode)
        {
            if (!this.IsLoading)
            {
                this.Winning = string.Empty;
                this.IsLoading = true;
                this.Winning = await this.lotteryTicketChecker.Check(barcode);
                this.IsLoading = false;
            }
        }
    }
}
