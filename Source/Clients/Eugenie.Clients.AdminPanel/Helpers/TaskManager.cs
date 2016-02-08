namespace Eugenie.Clients.AdminPanel.Helpers
{
    using System.Linq;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;

    using Common.Contracts;

    using Contracts;

    using Models;

    public class TaskManager : ITaskManager
    {
        private readonly ITasksStorage tasksStorage;
        private readonly IServerStorage serversStorage;
        private readonly IWebApiClient apiClient;

        public TaskManager(ITasksStorage tasksStorage, IServerStorage serversStorage, IWebApiClient apiClient)
        {
            this.tasksStorage = tasksStorage;
            this.serversStorage = serversStorage;
            this.apiClient = apiClient;

            this.RunAddOrUpdateProductTasks();
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
                                    var status = await this.apiClient.AddOrUpdateAsync(client, task.Model);
                                    if (status == HttpStatusCode.OK || status == HttpStatusCode.BadRequest)
                                    {
                                        this.tasksStorage.AddOrUpdateProductTasks.Remove(task);
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

                    Thread.Sleep(1000);
                    }
                });
        }
    }
}