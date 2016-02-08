namespace Eugenie.Clients.AdminPanel.Models
{
    public class DeleteProductTask
    {
        public DeleteProductTask(string serverName, string productName)
        {
            this.ServerName = serverName;
            this.ProductName = productName;
        }

        public string ServerName { get; set; }

        public string ProductName { get; set; }
    }
}