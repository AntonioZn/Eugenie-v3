namespace Eugenie.Clients.Common.Helpers
{
    using Newtonsoft.Json;

    public static class DeepCopier<T>
    {
        public static T Copy(T item)
        {
            var serialized = JsonConvert.SerializeObject(item);
            var deserialized = JsonConvert.DeserializeObject<T>(serialized);
            return deserialized;
        }
    }
}