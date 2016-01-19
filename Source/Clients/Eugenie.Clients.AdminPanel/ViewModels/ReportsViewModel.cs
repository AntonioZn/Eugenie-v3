﻿namespace Eugenie.Clients.AdminPanel.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Common.Contracts;
    using Common.WebApiModels;
    using Common.Еxtensions;

    using GalaSoft.MvvmLight;

    using Notifications;

    public class ReportsViewModel : ViewModelBase, IEnterHandler
    {
        private readonly IServerManager manager;
        private ObservableCollection<Report> reports; 

        public ReportsViewModel(IServerManager manager)
        {
            this.manager = manager;
            this.manager.ServerTestingFinished += this.OnServerTestingFinished;
        }

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

        private void OnServerTestingFinished(object sender, EventArgs e)
        {
            this.Reports = this.manager.Cache.ReportsPerServer.FirstOrDefault().Value;
        }

        public void HandleEnter()
        {
            NotificationsHost.Error("Not implemented", "This operation is not implemented yet!");
        }
    }
}