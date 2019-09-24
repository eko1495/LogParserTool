using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogParser.Interfaces;
using LogParser.Model;
using NLog;

namespace LogParser.Services
{
    public class ProcessService : IProcessService
    {
        private readonly ILogger _logger;

        public ProcessService(ILogger logger)
        {
            _logger = logger;
        }

        public EventData Create(LogLine currentLine, LogLine logLine)
        {
            EventData eventData = new EventData();
            eventData.Type = currentLine.Type;
            eventData.EventId = currentLine.Id;
            eventData.Host = currentLine.Host;

            if (currentLine.State == "FINISHED")
            {
                eventData.EventDuration = currentLine.Timestamp - logLine.Timestamp;
            }
            else if (currentLine.State == "STARTED")
            {
                eventData.EventDuration = logLine.Timestamp - currentLine.Timestamp;
            }
            else
            {
                _logger.Warn($"Unknown state: {currentLine.State}");
                return null;
            }

            eventData.Alert = eventData.EventDuration > 4;
            return eventData;
        }
    }
}
