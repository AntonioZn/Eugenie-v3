namespace Eugenie.Clients.AdminPanel.Views
{
    using ViewModels;

    public partial class Delivery
    {
        public Delivery()
        {
            InitializeComponent();
            var resource = this.TryFindResource("ViewModelLocator");
            var viewModelLocator = resource as ViewModelLocator;
            var viewModel = viewModelLocator.AddNewProductViewModel;
            this.DataContext = viewModel;
        }

        public Delivery(DeliveryViewModel model)
        {
            this.DataContext = model;
        }
    }
}