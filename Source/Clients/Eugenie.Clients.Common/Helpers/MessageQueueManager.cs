namespace Eugenie.Clients.Common.Helpers
{
    using System.Messaging;

    public class MessageQueueManager : IMessageQueueManager
    {
        public MessageQueueManager()
        {
            if (!MessageQueue.Exists(@".\Private$\EugenieProductUpdates"))
            {
                MessageQueue.Create(@".\Private$\EugenieProductUpdates");
            }

            this.MessageQueue = new MessageQueue(@".\Private$\EugenieProductUpdates");
            this.MessageQueue.Formatter = new JsonFormatter();
        }

        public MessageQueue MessageQueue { get; set; }
    }
}
