using System;
using LogParser.Interfaces;
using LogParser.Model;
using LogParser.Services;
using Moq;
using NLog;
using NUnit.Framework;

namespace LogParser.Tests
{
    [TestFixture]
    public class LogParserTest
    {
        [Test]
        public void TestJsonConverter1()
        {
            string json = "{\"id\":\"scsmbstgra\", \"state\":\"STARTED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495212}";
            IConvertService<LogLine> convertService = new JsonService<LogLine>();
            LogLine logLine = convertService.ToObject(json);

            Assert.That(logLine, Is.Not.Null);
            Assert.That(logLine.Id, Is.EqualTo("scsmbstgra"));
        }


        [Test]
        public void TestCreateEventData()
        {
            string json1 = "{\"id\":\"scsmbstgra\", \"state\":\"STARTED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495212}";
            string json2 = "{\"id\":\"scsmbstgra\", \"state\":\"FINISHED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495216}";

            IConvertService<LogLine> convertService = new JsonService<LogLine>();
            LogLine logLine1 = convertService.ToObject(json1);
            LogLine logLine2 = convertService.ToObject(json2);

            Mock<ILogger> logger = new Mock<ILogger>();

            IProcessService processService = new ProcessService(logger.Object);
            EventData eventData = processService.Create(logLine1, logLine2);
                                                                                                                                                                                                                                   
            Assert.That(eventData.EventDuration, Is.EqualTo(4));
        }

        [Test]
        public void TestCreateEventDataReversed()
        {
            string json1 = "{\"id\":\"scsmbstgra\", \"state\":\"FINISHED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495216}";
            string json2 = "{\"id\":\"scsmbstgra\", \"state\":\"STARTED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495212}";

            IConvertService<LogLine> convertService = new JsonService<LogLine>();
            LogLine logLine1 = convertService.ToObject(json1);
            LogLine logLine2 = convertService.ToObject(json2);

            Mock<ILogger> logger = new Mock<ILogger>();

            IProcessService processService = new ProcessService(logger.Object);
            EventData eventData = processService.Create(logLine1, logLine2);

            Assert.That(eventData.EventDuration, Is.EqualTo(4));
        }

        [Test]
        public void TestAlert()
        {
            string json1 = "{\"id\":\"scsmbstgra\", \"state\":\"FINISHED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495217}";
            string json2 = "{\"id\":\"scsmbstgra\", \"state\":\"STARTED\", \"type\":\"APPLICATION_LOG\", \"host\":\"12345\", \"timestamp\":1491377495212}";

            IConvertService<LogLine> convertService = new JsonService<LogLine>();
            LogLine logLine1 = convertService.ToObject(json1);
            LogLine logLine2 = convertService.ToObject(json2);

            Mock<ILogger> logger = new Mock<ILogger>();

            IProcessService processService = new ProcessService(logger.Object);
            EventData eventData = processService.Create(logLine1, logLine2);

            Assert.That(eventData.Alert, Is.EqualTo(true));
        }
    }
}
