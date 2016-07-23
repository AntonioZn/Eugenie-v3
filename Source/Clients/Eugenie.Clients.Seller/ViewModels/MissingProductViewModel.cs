namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Common.Contracts;

    using GalaSoft.MvvmLight;

    using MaterialDesignThemes.Wpf;

    public class MissingProductViewModel : ViewModelBase, IBarcodeHandler
    {
        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }
    }
}
