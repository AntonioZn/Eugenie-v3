﻿namespace Eugenie.Clients.Admin.Views
{
    using GalaSoft.MvvmLight;

    public partial class ReportDetails
    {
        public ReportDetails(ViewModelBase viewModel)
        {
            this.DataContext = viewModel;
            this.InitializeComponent();
        }
    }
}