namespace Eugenie.Clients.Seller.ViewModels
{
    using Common.Contracts;

    using Helpers;

    using Sv.Wpf.Core.Mvvm;

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
            get => this.winning;
            set => this.Set(ref this.winning, value);
        }

        public bool IsLoading
        {
            get => this.isLoading;
            set => this.Set(ref this.isLoading, value);
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
