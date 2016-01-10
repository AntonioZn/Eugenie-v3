namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;

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

            this.SaveChangesCommand = new RelayCommand(this.HandleSaveChangesCommand, this.CanSaveChanges);
            this.CancelChangesCommand = new RelayCommand(this.HandleCancelChangesCommand);
        }

        public RelayCommand SaveChangesCommand { get; set; }

        public RelayCommand CancelChangesCommand { get; set; }

        private void HandleCancelChangesCommand()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void HandleSaveChangesCommand()
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private bool CanSaveChanges()
        {
            return this.MainProductViewModel.Product.Name.Length > 2;
        }

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers { get; set; }

        public ProductViewModel MainProductViewModel { get; set; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();
    }
}