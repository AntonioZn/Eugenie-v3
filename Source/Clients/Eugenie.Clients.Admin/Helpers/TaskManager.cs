namespace Eugenie.Clients.Admin.Helpers
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Windows;
    
    using Common.Notifications;

    using Contracts;

    using Models;

    public class TaskManager : ITaskManager
    {
        private readonly IServerStorage serversStorage;
        private readonly ITasksStorage tasksStorage;

        public TaskManager(ITasksStorage tasksStorage, IServerStorage serversStorage)
        {
            this.tasksStorage = tasksStorage;
            this.serversStorage = serversStorage;

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
            Task.Run(async () =>
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
                                             var status = await client.AddOrUpdateAsync(task.Model);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                                                       {
                                                                                           this.tasksStorage.AddOrUpdateProductTasks.Remove(task);
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

        private void RunDeleteProductTasks()
        {
            Task.Run(async () =>
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
                                             var status = await client.DeleteProductAsync(task.ProductName);
                                             if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                             {
                                                 Application.Current.Dispatcher.Invoke(() =>
                                                 {
                                                     this.tasksStorage.DeleteProductTasks.Remove(task);
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