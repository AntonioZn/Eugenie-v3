namespace Eugenie.Clients.Seller.Views
{
    public partial class LotteryTicketChecker
    {
        public LotteryTicketChecker(object viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
