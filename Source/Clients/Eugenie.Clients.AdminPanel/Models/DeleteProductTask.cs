namespace Eugenie.Clients.AdminPanel.Models
{
    using Common.Models;

    public class DeleteProductTask
    {
        public DeleteProductTask(ServerInformation serverInformation, string productName)
        {
            this.ServerInformation = serverInformation;
            this.ProductName = productName;
        }

        public ServerInformation ServerInformation { get; set; }

        public string ProductName { get; set; }
    }
}