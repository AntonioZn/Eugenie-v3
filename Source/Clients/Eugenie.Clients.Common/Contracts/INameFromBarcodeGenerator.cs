namespace Eugenie.Clients.Common.Contracts
{
    public interface INameFromBarcodeGenerator
    {
        string GetName(string barcode);
    }
}