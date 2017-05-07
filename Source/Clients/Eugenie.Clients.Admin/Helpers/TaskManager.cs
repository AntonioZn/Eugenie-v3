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
                             var groups = this.AddOrUpdateProductTasks.GroupBy(x => x.ServerName);
                             foreach (var group in groups)
                             {
                                 foreach (var task in group)
                                 {
                                     var client = this.stores.FirstOrDefault(x => x.Name == task.ServerName)?.Client;
                                     if (client != null)
                                     {
                                         try
                                         {
                                             var status = await client.AddOrUpdateAsync(task.Model);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                                                       {
                                                                                           this.AddOrUpdateProductTasks.Remove(task);
                                                                                           NotificationsHost.Success(task.ServerName, $"{task.Model.Name} е записан успешно.");
                                                                                       });
                                             }
                                         }
                                         catch
                                         {
                                         }
                                     }
                                     else
                                     {
                                         break;
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
                             var groups = this.DeleteProductTasks.GroupBy(x => x.ServerName);
                             foreach (var group in groups)
                             {
                                 foreach (var task in group)
                                 {
                                     var client = this.stores.FirstOrDefault(x => x.Name == task.ServerName)?.Client;
                                     if (client != null)
                                     {
                                         try
                                         {
                                             var status = await client.DeleteProductAsync(task.ProductName);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                                                       {
                                                                                           this.DeleteProductTasks.Remove(task);
                                                                                           NotificationsHost.Success(task.ServerName, $"{task.ProductName} е изтрит успешно.");
                                                                                       });
                                             }
                                         }
                                         catch
                                         {
                                         }
                                     }
                                     else
                                     {
                                         break;
                                     }
                                 }
                             }

                             await Task.Delay(2000);
                         }
                     });
        }
    }
}