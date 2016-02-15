namespace Eugenie.Clients.Admin.Helpers
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    using Common.Contracts;
    using Common.Notifications;

    using Contracts;

    using Models;

    public class TaskManager : ITaskManager
    {
        private readonly IWebApiClient apiClient;
        private readonly IServerStorage serversStorage;
        private readonly ITasksStorage tasksStorage;

        public TaskManager(ITasksStorage tasksStorage, IServerStorage serversStorage, IWebApiClient apiClient)
        {
            this.tasksStorage = tasksStorage;
            this.serversStorage = serversStorage;
            this.apiClient = apiClient;

            this.RunAddOrUpdateProductTasks();
            this.RunDeleteProductTasks();
        }

        public void AddTask(AddOrUpdateProductTask task)
        {
            this.tasksStorage.AddOrUpdateProductTasks.Add(task);
        }

        public void AddTask(DeleteProductTask task)
        {
            this.tasksStorage.DeleteProductTasks.Add(task);
        }

        private void RunAddOrUpdateProductTasks()
        {
            Task.Run(() =>
                     {
                         while (true)
                         {
                             var groups = this.tasksStorage.AddOrUpdateProductTasks.GroupBy(x => x.ServerName);
                             foreach (var group in groups)
                             {
                                 foreach (var task in group)
                                 {
                                     var client = this.serversStorage.Servers.FirstOrDefault(x => x.Name == task.ServerName)?.Client;
                                     if (client != null)
                                     {
                                         try
                                         {
                                             var status = this.apiClient.AddOrUpdateAsync(client, task.Model).Result;
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 this.tasksStorage.AddOrUpdateProductTasks.Remove(task);
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                 {
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

                             Thread.Sleep(2000);
                         }
                     });
        }

        private void RunDeleteProductTasks()
        {
            Task.Run(() =>
                     {
                         while (true)
                         {
                             var groups = this.tasksStorage.DeleteProductTasks.GroupBy(x => x.ServerName);
                             foreach (var group in groups)
                             {
                                 foreach (var task in group)
                                 {
                                     var client = this.serversStorage.Servers.FirstOrDefault(x => x.Name == task.ServerName)?.Client;
                                     if (client != null)
                                     {
                                         try
                                         {
                                             var status = this.apiClient.DeleteProductAsync(client, task.ProductName).Result;
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 this.tasksStorage.DeleteProductTasks.Remove(task);
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

                             SpinWait.SpinUntil(() => this.tasksStorage.DeleteProductTasks.Any());
                         }
                     });
        }
    }
}