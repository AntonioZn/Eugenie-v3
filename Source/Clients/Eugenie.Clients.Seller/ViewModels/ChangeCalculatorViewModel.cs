namespace Eugenie.Clients.Seller.ViewModels
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Autofac;

    using Common.Contracts;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Mvvm;

    public class ChangeCalculatorViewModel : ViewModelBase, IBarcodeHandler
    {
        private static readonly string delimiter = Convert.ToString(CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

        private decimal change;
        private string payment;
        private decimal totalPrice;

        public decimal TotalPrice
        {
            get => this.totalPrice;
            set => this.Set(ref this.totalPrice, value);
        }

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
                this.payment = value?.Replace(",", delimiter).Replace(".", delimiter).Replace($"{delimiter}{delimiter}", delimiter);
                if (decimal.TryParse(this.payment, out var result))
                {
                    this.Change = result - this.TotalPrice;
                }
                else
                {
                    this.Change = 0;
                }
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            this.TotalPrice = (decimal) navigationData;
            return Task.CompletedTask;
        }

        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }
    }
}