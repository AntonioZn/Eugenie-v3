namespace Eugenie.Clients.Common.Helpers
{
    using System;
    using System.IO;
    using System.Messaging;
    using System.Text;
    using Newtonsoft.Json;

    public class JsonFormatter : IMessageFormatter
    {
        public bool CanRead(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            var stream = message.BodyStream;

            return stream != null
                && stream.CanRead
                && stream.Length > 0;
        }

        public object Clone()
        {
            return new JsonFormatter();
        }

        public object Read(Message message)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (CanRead(message) == false)
                return null;

            using (var reader = new StreamReader(message.BodyStream, Encoding.UTF8))
            {
                var json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject(json);
            }
        }

        public void Write(Message message, object obj)
        {
            if (message == null)
                throw new ArgumentNullException("message");

            if (obj == null)
                throw new ArgumentNullException("obj");

            string json = JsonConvert.SerializeObject(obj, Formatting.None);

            message.BodyStream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            
            message.BodyType = 0;
        }
    }
}