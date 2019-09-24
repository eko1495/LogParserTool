using LiteDB;

namespace LogParser.Model
{
    public class EventData
    {
        [BsonId]
        public string EventId { get; set; }
        public long EventDuration { get; set; }
        public string Type { get; set; }
        public string Host { get; set; }
        public bool Alert { get; set; }
    }
}
