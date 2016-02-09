namespace Eugenie.Clients.Admin.Contracts
{
    using System.Collections.ObjectModel;

    using Models;

    public interface ITasksStorage
    {
        ObservableCollection<AddOrUpdateProductTask> AddOrUpdateProductTasks { get; }

        ObservableCollection<DeleteProductTask> DeleteProductTasks { get; }
    }
}