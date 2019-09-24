using System;
using LogParser.Interfaces;
using Newtonsoft.Json;
using NLog;

namespace LogParser.Services
{
    public class JsonService<T> : IConvertService<T> where T : new()
    {
        private readonly ILogger _logger;

        public JsonService(ILogger logger)
        {
            _logger = logger;
        }
        public T ToObject(string line)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(line);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                throw;
            }
        }
    }
}
