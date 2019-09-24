using LogParser.Interfaces;
using Newtonsoft.Json;

namespace LogParser.Services
{
    public class JsonService<T> : IConvertService<T> where T : new()
    {
        public T ToObject(string line)
        {
            return JsonConvert.DeserializeObject<T>(line);
        }
    }
}
