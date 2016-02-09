namespace Eugenie.Clients.Admin.Contracts
{
    using Models;

    public interface ITaskManager
    {
        void AddTask(DeleteProductTask task);

        void AddTask(AddOrUpdateProductTask task);
    }
}