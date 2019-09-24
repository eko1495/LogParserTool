using LogParser.Model;

namespace LogParser.Interfaces
{
    public interface IProcessService
    {
        EventData Create(LogLine currentLine, LogLine logLine);
    }
}