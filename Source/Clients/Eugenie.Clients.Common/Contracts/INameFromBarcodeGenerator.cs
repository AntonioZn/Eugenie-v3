namespace Eugenie.Clients.Common.Contracts
{
    using System.Threading.Tasks;

    public interface INameFromBarcodeGenerator
    {
        Task<string> GetName(string barcode);
    }
}