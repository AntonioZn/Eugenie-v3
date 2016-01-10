namespace Eugenie.Clients.Common.Еxtensions
{
    using Newtonsoft.Json;

    public static class GenericExtensions
    {
        public static T DeepClone<T>(this T item)
        {
            var serialized = JsonConvert.SerializeObject(item);
            var deserialized = JsonConvert.DeserializeObject<T>(serialized);
            return deserialized;
        }
    }
}