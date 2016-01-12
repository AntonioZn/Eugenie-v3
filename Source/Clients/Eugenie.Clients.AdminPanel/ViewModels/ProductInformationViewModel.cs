namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows.Input;

    using Common.Helpers;
    using Common.Models;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignThemes.Wpf;

    public class ProductInformationViewModel : ViewModelBase
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, ProductViewModel> productInAllServers, ProductViewModel mainMainProductViewModel)
        {
            this.ProductInAllServers = productInAllServers;
            this.MainProductViewModel = mainMainProductViewModel;

            this.SaveCommand = new RelayCommand(this.HandleSaveCommand, this.CanSave);
            this.CancelCommand = new RelayCommand(this.HandleCancelCommand);
        }

        public ICommand CancelCommand { get; set; }

        public ICommand SaveCommand { get; set; }

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers { get; set; }

        public ProductViewModel MainProductViewModel { get; set; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        private void HandleCancelCommand()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void HandleSaveCommand()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private bool CanSave()
        {
            return this.MainProductViewModel.Product.HasNoValidationErrors()
                && this.MainProductViewModel.HasNoValidationErrors()
                && this.ProductInAllServers.Values.All(x => x.HasNoValidationErrors());
        }
    }
}