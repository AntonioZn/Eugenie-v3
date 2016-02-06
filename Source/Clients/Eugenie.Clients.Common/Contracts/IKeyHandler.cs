namespace Eugenie.Clients.Common.Contracts
{
    using System.Windows.Input;

    public interface IKeyHandler
    {
        void HandleKey(KeyEventArgs e, Key key);
    }
}