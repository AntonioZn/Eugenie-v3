namespace Eugenie.Clients.Seller.ViewModels
{
    using System.Windows.Input;
    
    using Common.Contracts.KeyHandlers;
    using Common.Models;

    using GalaSoft.MvvmLight;

    public class SellViewModel : ViewModelBase, IBarcodeHandler, IDeleteHandler, IEnterHandler, IEscapeHandler
    {
        public SellViewModel()
        {
            this.FullName = "Svetlozar Stoichkov";
            this.Basket = new BasketViewModel();
        }

        public ICommand Enter { get; }

        public string FullName { get; set; }

        public BasketViewModel Basket { get; }

        public Product SelectedProduct { get; set; }

        public void HandleBarcode(string barcode)
        {
            throw new System.NotImplementedException();
        }

        public void HandleDelete()
        {
            this.Basket.Delete(this.SelectedProduct);
        }

        public void HandleEnter()
        {
            throw new System.NotImplementedException();
        }

        public void HandleEscape()
        {
            throw new System.NotImplementedException();
        }
    }
}