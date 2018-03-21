namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Common.Contracts;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Mvvm;

    public class ChangeCalculatorViewModel : ViewModelBase, IBarcodeHandler
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
            get => this.change;

            set => this.Set(ref this.change, value);
        }

        public string Payment
        {
            get => this.payment;

            set
            {
                this.payment = value;
                decimal result;
                decimal.TryParse(value, out result);
                this.Change = result - this.TotalPrice;
            }
        }

        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }
    }
}