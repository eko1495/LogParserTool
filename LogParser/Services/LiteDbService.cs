using System.Collections.Generic;
using LiteDB;
using LogParser.Interfaces;
using NLog;
using Logger = LiteDB.Logger;

namespace LogParser.Services
{
    internal class LiteDbService : ILiteDbService
    {
        private readonly ILogger _logger;

        public LiteDbService(ILogger logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// Write batch of records to database
        /// </summary>
        /// <typeparam name="T">Type of </typeparam>
        /// <param name="collectionName"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int WriteBulk<T>(string collectionName, List<T> list)
        {
            using (var db = new LiteDatabase("Filename=LogParser.db; Mode=Exclusive;", null, new Logger(Logger.FULL, (o)=>{_logger.Debug(o);})))
            {
                LiteCollection<T> logs = db.GetCollection<T>(collectionName);

                return logs.InsertBulk(list);
            }
        }
    }
}
