namespace Eugenie.Clients.Common.Helpers
{
    using System.Messaging;

    public interface IMessageQueueManager
    {
        MessageQueue MessageQueue { get; set; }
    }
}