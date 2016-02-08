namespace Eugenie.Clients.Common.Contracts
{
    using System.ComponentModel;

    public interface IValidatableObject : IDataErrorInfo
    {
        bool HasNoValidationErrors();
    }
}