using System;
using LiteDB;
using Newtonsoft.Json;

namespace LogParser.Model
{
    public class LogLine
    {
        [BsonId, JsonIgnore]
        public Guid DbId { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
    }
}
