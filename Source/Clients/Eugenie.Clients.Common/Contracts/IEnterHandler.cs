namespace Eugenie.Clients.Common.Contracts
{
    using System.Windows.Input;

    public interface IEnterHandler
    {
        void HandleEnter();

        ICommand HandleEnterCommand { get; }
    }
}