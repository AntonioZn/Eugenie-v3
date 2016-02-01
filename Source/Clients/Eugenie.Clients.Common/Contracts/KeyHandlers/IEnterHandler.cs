namespace Eugenie.Clients.Common.Contracts.KeyHandlers
{
    using System.Windows.Input;

    public interface IEnterHandler
    {
        void HandleEnter();

        ICommand Enter { get; }
    }
}