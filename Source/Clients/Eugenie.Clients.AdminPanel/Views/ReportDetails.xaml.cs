﻿namespace Eugenie.Clients.AdminPanel.Views
{
    using GalaSoft.MvvmLight;

    public partial class ReportDetails
    {
        public ReportDetails(ViewModelBase viewModel)
        {
            this.InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
