namespace Eugenie.Clients.Admin.Models
{
    public class DeleteProductTask
    {
        public DeleteProductTask(string serverId, string productName)
        {
            this.ServerId = serverId;
            this.ProductName = productName;
        }

        public string ServerId { get; set; }

        public string ProductName { get; set; }
    }
}