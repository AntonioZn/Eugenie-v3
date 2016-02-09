namespace Eugenie.Clients.Seller.ViewModels
{
    using GalaSoft.MvvmLight;

    public class ChangeCalculatorViewModel : ViewModelBase
    {
        private decimal change;
        private string payment;

        public ChangeCalculatorViewModel(decimal totalPrice)
        {
            this.TotalPrice = totalPrice;
        }

        public decimal TotalPrice { get; set; }

        public decimal Change
        {
            get
            {
                return this.change;
            }

            set
            {
                this.Set(() => this.Change, ref this.change, value);
            }
        }

        public string Payment
        {
            get
            {
                return this.payment;
            }

            set
            {
                this.payment = value;
                decimal result;
                decimal.TryParse(value, out result);
                this.Change = result - this.TotalPrice;
            }
        }
    }
}