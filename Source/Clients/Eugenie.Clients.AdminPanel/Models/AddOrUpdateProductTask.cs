namespace Eugenie.Clients.AdminPanel.Models
{
    using Common.Models;
    using Common.WebApiModels;

    public class AddOrUpdateProductTask
    {
        public AddOrUpdateProductTask(ServerInformation serverInformation, AddProductModel model)
        {
            this.ServerInformation = serverInformation;
            this.Model = model;
        }

        public ServerInformation ServerInformation { get; }

        public AddProductModel Model { get; }    
    }
}