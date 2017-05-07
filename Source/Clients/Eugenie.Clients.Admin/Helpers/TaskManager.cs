namespace Eugenie.Clients.Admin.Helpers
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;

    using Common.Notifications;

    using Models;

    using Newtonsoft.Json;

    using Properties;

    public class TaskManager
    {
        private readonly Settings settings;
        private IEnumerable<Store> stores;

        public TaskManager(Settings settings)
        {
            this.settings = settings;
            this.AddOrUpdateProductTasks = JsonConvert.DeserializeObject<ObservableCollection<AddOrUpdateProductTask>>(settings.AddOrUpdateProductTasks);
            this.DeleteProductTasks = JsonConvert.DeserializeObject<ObservableCollection<DeleteProductTask>>(settings.DeleteProductTasks);

            this.AddOrUpdateProductTasks.CollectionChanged += (s, e) =>
                                                              {
                                                                  settings.AddOrUpdateProductTasks = JsonConvert.SerializeObject(this.AddOrUpdateProductTasks);
                                                                  settings.Save();
                                                              };

            this.DeleteProductTasks.CollectionChanged += (s, e) =>
                                                         {
                                                             settings.DeleteProductTasks = JsonConvert.SerializeObject(this.DeleteProductTasks);
                                                             settings.Save();
                                                         };
        }

        public ObservableCollection<AddOrUpdateProductTask> AddOrUpdateProductTasks { get; }

        public ObservableCollection<DeleteProductTask> DeleteProductTasks { get; }

        public void Start(IEnumerable<Store> stores)
        {
            this.stores = stores;
            this.RunAddOrUpdateProductTasks();
            this.RunDeleteProductTasks();
        }

        private void RunAddOrUpdateProductTasks()
        {
            Task.Run(async () =>
                     {
                         while (true)
                         {
                             var groups = this.AddOrUpdateProductTasks.GroupBy(x => x.ServerId);
                             foreach (var group in groups)
                             {
                                 var store = this.stores.FirstOrDefault(x => x.Id == group.Key);
                                 var client = store?.Client;

                                 if (client != null)
                                 {
                                     foreach (var task in group)
                                     {
                                         try
                                         {
                                             var status = await client.AddOrUpdateAsync(task.Model);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                                                       {
                                                                                           this.AddOrUpdateProductTasks.Remove(task);
                                                                                           NotificationsHost.Success(store.Name, $"{task.Model.Name} е записан успешно.");
                                                                                       });
                                             }
                                         }
                                         catch
                                         {
                                             break;
                                         }
                                     }
                                 }
                             }

                             await Task.Delay(2000);
                         }
                     });
        }

        public void RunDeleteProductTasks()
        {
            Task.Run(async () =>
                     {
                         while (true)
                         {
                             var groups = this.DeleteProductTasks.GroupBy(x => x.ServerId);
                             foreach (var group in groups)
                             {
                                 var store = this.stores.FirstOrDefault(x => x.Id == group.Key);
                                 var client = store?.Client;

                                 if (client != null)
                                 {
                                     foreach (var task in group)
                                     {
                                         try
                                         {
                                             var status = await client.DeleteProductAsync(task.ProductName);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                                                       {
                                                                                           this.DeleteProductTasks.Remove(task);
                                                                                           NotificationsHost.Success(store.Name, $"{task.ProductName} е изтрит успешно.");
                                                                                       });
                                             }
                                         }
                                         catch
                                         {
                                             break;
                                         }
                                     }
                                 }
                             }

                             await Task.Delay(2000);
                         }
                     });
        }
    }
}