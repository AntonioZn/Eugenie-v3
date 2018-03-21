namespace Eugenie.Clients.Seller.ViewModels
{
    using Autofac;

    using Common.Contracts;

    using MaterialDesignThemes.Wpf;

    using Sv.Wpf.Core.Mvvm;

    public class MissingProductViewModel : ViewModelBase, IBarcodeHandler
    {
        public void HandleBarcode(string barcode)
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
            ViewModelLocator.Container.Resolve<MainWindowViewModel>().HandleBarcode(barcode);
        }
    }
}
