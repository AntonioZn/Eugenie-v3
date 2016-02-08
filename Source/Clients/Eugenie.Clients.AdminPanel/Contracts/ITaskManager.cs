namespace Eugenie.Clients.AdminPanel.Contracts
{
    using Models;

    public interface ITaskManager
    {
        void AddTask(DeleteProductTask task);

        void AddTask(AddOrUpdateProductTask task);
    }
}