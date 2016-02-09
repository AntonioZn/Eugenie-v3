namespace Eugenie.Clients.Admin.Models
{
    using Common.WebApiModels;

    public class AddOrUpdateProductTask
    {
        public AddOrUpdateProductTask(string serverName, AddProductModel model)
        {
            this.ServerName = serverName;
            this.Model = model;
        }

        public string ServerName { get; }

        public AddProductModel Model { get; }
    }
}