namespace Eugenie.Clients.AdminPanel.Views
{
    using ViewModels;

    public partial class ReportDetails
    {
        public ReportDetails(ReportDetailsViewModel model)
        {
            this.InitializeComponent();
            this.DataContext = model;
        }
    }
}
