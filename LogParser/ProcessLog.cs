using System;
using System.Collections.Generic;
using System.IO;
using LogParser.Helpers;
using LogParser.Interfaces;
using LogParser.Model;
using LogParser.Services;
using NLog;

namespace LogParser
{
    public class ProcessLog
    {
        private readonly string _filePath;
        private readonly IConvertService<LogLine> _convertService;
        private readonly ILogger _logger;
        private readonly IProcessService _processService;

        public ProcessLog(string filePath, IConvertService<LogLine> convertService, ILogger logger, IProcessService processService)
        {
            _filePath = filePath;
            _convertService = convertService;
            _logger = logger;
            _processService = processService;
        }

        /// <summary>
        /// Run log file processing
        /// </summary>
        public void Run()
        {
            BulkHelper<EventData> blkHelper = new BulkHelper<EventData>("logs"+DateTime.UtcNow.ToString("YYYYMMddHHmmss"), new LiteDbService(_logger), _logger);

            using (FileStream fs = File.Open(_filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            using (StreamReader sr = new StreamReader(fs))
            {
                Dictionary<string, LogLine> lines = new Dictionary<string, LogLine>();

                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    LogLine currentLine = _convertService.ToObject(line);

                    LogLine logLine;
                    if (lines.TryGetValue(currentLine.Id, out logLine))
                    {
                        EventData eventData = _processService.Create(currentLine, logLine);

                        if (eventData == null)
                            continue;

                        blkHelper.QueueInsert(eventData);
                        lines.Remove(currentLine.Id);
                    }
                    else
                    {
                        lines.Add(currentLine.Id, currentLine);
                    }

                }
                blkHelper.QueuePush();
            }

            _logger.Info($"File processed.");
        }

        
    }
}
