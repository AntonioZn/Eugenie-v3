namespace Eugenie.Clients.Admin.Models
{
    using Common.WebApiModels;

    public class AddOrUpdateProductTask
    {
        public AddOrUpdateProductTask(string serverId, AddProductModel model)
        {
            this.ServerId = serverId;
            this.Model = model;
        }

        public string ServerId { get; }

        public AddProductModel Model { get; }
    }
}