namespace Eugenie.Clients.Common.Models
{
    using WebApiModels;

    public class ServerAddProductPair
    {
        public ServerAddProductPair(ServerInformation serverInformation, AddProductModel addProductModel)
        {
            this.ServerInformation = serverInformation;
            this.AddProductModel = addProductModel;
        }

        public ServerInformation ServerInformation { get; set; }

        public AddProductModel AddProductModel { get; set; }    
    }
}