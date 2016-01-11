namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System.Collections.Generic;
    using System.Windows.Controls;
    using System.Windows.Input;

    using Common.Helpers;
    using Common.Models;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;

    using MaterialDesignThemes.Wpf;

    public class ProductInformationViewModel : ViewModelBase
    {
        public ProductInformationViewModel(IDictionary<ServerInformation, ProductViewModel> productInAllServers, ProductViewModel mainMainProductViewModel)
        {
            this.ProductInAllServers = productInAllServers;
            this.MainProductViewModel = mainMainProductViewModel;

            this.SaveChangesCommand = new RelayCommand<UserControl>(this.HandleSaveChangesCommand, this.CanSaveChanges);
            this.CancelChangesCommand = new RelayCommand(this.HandleCancelChangesCommand);
        }

        public ICommand CancelChangesCommand { get; set; }

        public ICommand SaveChangesCommand { get; set; }

        public IDictionary<ServerInformation, ProductViewModel> ProductInAllServers { get; set; }

        public ProductViewModel MainProductViewModel { get; set; }

        public IEnumerable<MeasureType> Measures => MeasureTypeMapper.GetTypes();

        private void HandleCancelChangesCommand()
        {
            DialogHost.CloseDialogCommand.Execute(false, null);
        }

        private void HandleSaveChangesCommand(UserControl obj)
        {
            DialogHost.CloseDialogCommand.Execute(true, null);
        }

        private bool CanSaveChanges(UserControl arg)
        {
            return arg.HasNoValidationErrors();
        }
    }
}