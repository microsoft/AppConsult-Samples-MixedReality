namespace MsGraph.Library
{
    public interface IJsonService
    {
        T Deserialize<T>(string json) where T : new();
        string Serialize<T>(T item);
    }

    //it's needed since there are compatibility issues with json.net in unity
    //introduce some kind of dependency (json.net preferably), i guess. for now, idc.
    public class JsonService : IJsonService
    {
        public string Serialize<T>(T item)
        {
            return string.Empty;
        }

        public T Deserialize<T>(string json) where T : new()
        {
            return new T();
        }
    }
}
