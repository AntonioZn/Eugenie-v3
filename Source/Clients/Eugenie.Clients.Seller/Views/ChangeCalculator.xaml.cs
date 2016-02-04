namespace Eugenie.Clients.Seller.Views
{
    using GalaSoft.MvvmLight;

    public partial class ChangeCalulator
    {
        public ChangeCalulator(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}