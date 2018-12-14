using NJ = Newtonsoft.Json;

namespace MP.WindowsServices.Common.Serializer
{
    public class JsonSerializer : ISerializer
    {
        public T Deserialize<T>(string serializedData)
        {
            return NJ.JsonConvert.DeserializeObject<T>(serializedData);
        }

        public string Serialize<T>(T data)
        {
            return NJ.JsonConvert.SerializeObject(data);
        }
    }
}
