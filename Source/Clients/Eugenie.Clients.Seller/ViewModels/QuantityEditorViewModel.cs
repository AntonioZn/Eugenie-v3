namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Windows.Input;

    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    public class QuantityEditorViewModel : ViewModelBase
    {
        private readonly decimal minimumQuantity;
        private readonly MeasureType measure;
        private string quantity;

        public QuantityEditorViewModel(decimal startingQuantity, decimal minimumQuantity, MeasureType measure, string name)
        {
            this.minimumQuantity = minimumQuantity;
            this.measure = measure;
            this.Name = name;
            this.Quantity = startingQuantity.ToString();

            this.Add = new RelayCommand(this.HandleAdd);
            this.Cancel = new RelayCommand(this.HandleCancel);
        }

        public string Name { get; set; }

        public ICommand Add { get; }

        public ICommand Cancel { get; set; }

        public string Quantity
        {
            get
            {
                return this.quantity;
            }

            set
            {
                this.Set(() => this.Quantity, ref this.quantity, this.RestrictQuantity(value));
            }
        }

        private void HandleAdd()
        {
            decimal result;
            if (decimal.TryParse(this.Quantity, out result))
            {
                if (result >= this.minimumQuantity)
                {
                    DialogHost.CloseDialogCommand.Execute(true, null);
                }
            }
        }

        private void HandleCancel()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private string RestrictQuantity(string userInput)
        {
            var result = userInput.Replace("-", string.Empty);

            if (this.measure == MeasureType.бр)
            {
                result = result.Replace(",", string.Empty);

                //TODO: review coupon handling
                //if (StaticInfo.SellPanelViewModel.SearchedProduct.Name.Contains("$"))
                //{
                //    var breadCount = StaticInfo.SellPanelViewModel.Products
                //        .Where(x => x.Name.StartsWith("хляб") && x.SellingPrice >= StaticInfo.SellPanelViewModel.SearchedProduct.SellingPrice * -1).Sum(x => x.Quantity);
                //    try
                //    {
                //        if (int.Parse(result) > breadCount)
                //        {
                //            result = breadCount.ToString();
                //        }
                //    }
                //    catch
                //    {
                //    }
                //}
            }
            else
            {
                result = result.Replace(",,", ",");
            }

            return result;
        }
    }
}