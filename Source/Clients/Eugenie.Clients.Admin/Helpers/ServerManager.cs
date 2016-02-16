﻿namespace Eugenie.Clients.Admin.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Common.Contracts;
    using Common.Helpers;
    using Common.Models;
    using Common.WebApiModels;

    using Contracts;

    using Models;

    using ViewModels;

    public class ServerManager : IServerManager
    {
        private readonly IWebApiClient apiClient;
        private readonly IServerStorage storage;
        private readonly ITaskManager taskManager;

        public ServerManager(IServerStorage storage, IWebApiClient apiClient, ITaskManager taskManager)
        {
            this.storage = storage;
            this.storage.Servers.CollectionChanged += (s, e) => { this.Initialize(); };
            this.apiClient = apiClient;
            this.taskManager = taskManager;
            this.Cache = new Cache();

            this.Initialize();
        }

        public event EventHandler SelectedServerChanged;

        public event EventHandler ServerTestingFinished;

        public event EventHandler ProductsCacheChanged;

        public Cache Cache { get; }

        public ServerInformation SelectedServer { get; private set; }

        public void SetSelectedServer(string name)
        {
            if (this.SelectedServer?.Name != name)
            {
                this.SelectedServer = this.storage.Servers.FirstOrDefault(x => x.Name == name);
                this.SelectedServerChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        // TODO: add a way to cancel
        public async void Initialize()
        {
            this.Cache.ProductsPerServer.Clear();
            this.Cache.ReportsPerServer.Clear();
            this.Cache.MissingProductsPerServer.Clear();
            this.Cache.SellersPerServer.Clear();

            await Task.Run(() =>
                           {
                               Parallel.ForEach(this.storage.Servers, server =>
                                                                      {
                                                                          server.Client = ServerTester.TestServerAsync(server).Result;
                                                                          this.Cache.ProductsPerServer.Add(server, new List<Product>());
                                                                          this.Cache.ReportsPerServer.Add(server, new List<Report>());
                                                                          this.Cache.MissingProductsPerServer.Add(server, new List<MissingProduct>());
                                                                          this.Cache.SellersPerServer.Add(server, new List<Seller>());

                                                                          if (server.Client != null)
                                                                          {
                                                                              this.Cache.MissingProductsPerServer[server] = this.apiClient.GetMissingProductsAsync(server.Client).Result;
                                                                              this.Cache.ProductsPerServer[server] = this.apiClient.GetProductsAsync(server.Client).Result;
                                                                              this.Cache.ReportsPerServer[server] = this.apiClient.GetReportsAsync(server.Client).Result;
                                                                              this.Cache.SellersPerServer[server] = this.apiClient.GetSellersAsync(server.Client).Result;
                                                                          }
                                                                      });
                           });

            this.SetSelectedServer(string.Empty);
            this.ServerTestingFinished?.Invoke(this, EventArgs.Empty);
        }

        public void AddOrUpdate(IDictionary<ServerInformation, ProductViewModel> productInAllServers, ProductViewModel mainProductViewModel)
        {
            var cacheChanged = false;
            foreach (var pair in productInAllServers)
            {
                pair.Value.MapProperties(mainProductViewModel);

                var serverName = pair.Key.Name;
                var model = pair.Value.GetModel();
                var task = new AddOrUpdateProductTask(serverName, model);
                this.taskManager.AddTask(task);

                if (this.Cache.ProductsPerServer[pair.Key].All(x => x.Name != pair.Value.Product.Name))
                {
                    cacheChanged = true;
                    this.Cache.ProductsPerServer[pair.Key].Add(pair.Value.Product);
                }
            }

            if (cacheChanged)
            {
                this.ProductsCacheChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Delete(string productName)
        {
            foreach (var pair in this.Cache.ProductsPerServer)
            {
                this.taskManager.AddTask(new DeleteProductTask(pair.Key.Name, productName));
                var product = pair.Value.FirstOrDefault(x => x.Name == productName);
                if (product != null)
                {
                    pair.Value.Remove(product);
                }
            }

            this.ProductsCacheChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}