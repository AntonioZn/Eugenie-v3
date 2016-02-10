﻿namespace Eugenie.Clients.Admin.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using Autofac;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using Contracts;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using MaterialDesignThemes.Wpf;

    using Views;

    public class ReportsViewModel : ViewModelBase, IKeyHandler
    {
        private readonly IServerManager manager;
        private ObservableCollection<Report> reports;

        public ReportsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.SelectedServerChanged += this.OnSelectedServerChanged;

            this.Enter = new RelayCommand(this.HandleEnter);
        }

        public ICommand Enter { get; }

        public Report SelectedReport { get; set; }

        public IEnumerable<Report> Reports
        {
            get
            {
                return this.reports ?? (this.reports = new ObservableCollection<Report>());
            }

            set
            {
                this.reports = this.reports ?? new ObservableCollection<Report>();
                this.reports.Clear();
                value.ForEach(this.reports.Add);
            }
        }

        public void HandleKey(KeyEventArgs e, Key key)
        {
            switch (key)
            {
                case Key.Enter:
                    this.HandleEnter();
                    e.Handled = true;
                    break;
            }
        }

        public async void HandleEnter()
        {
            var viewModel = new ReportDetailsViewModel(ViewModelLocator.container.Resolve<IWebApiClient>(),
                                                       this.SelectedReport.Date, this.manager.SelectedServer.Client);
            await DialogHost.Show(new ReportDetails(viewModel), "RootDialog");
        }

        private void OnSelectedServerChanged(object sender, EventArgs e)
        {
            this.Reports = this.manager.SelectedServer == null ? Enumerable.Empty<Report>() : this.manager.Cache.ReportsPerServer[this.manager.SelectedServer];
        }
    }
}