using System;
using System.Collections.Generic;
using System.Linq;
using LogParser.Interfaces;
using NLog;

namespace LogParser.Helpers
{
    internal class BulkHelper<T> where T : new()
    {
        private readonly string _collectionName;
        private readonly ILiteDbService _dbService;
        private readonly ILogger _logger;
        private readonly List<T> _list = new List<T>();
        public BulkHelper(string collectionName, ILiteDbService dbService, ILogger logger)
        {
            _collectionName = collectionName;
            _dbService = dbService;
            _logger = logger;
        }

        /// <summary>
        /// Add to buffer for better performance
        /// </summary>
        /// <param name="data"></param>
        public void QueueInsert(T data)
        {
            _list.Add(data);


            if (_list.Count >= 5000)
            {
                _logger.Debug($"BulkHelper.QueueInsert QueueFull");
                int inserted = _dbService.WriteBulk(_collectionName, _list);

                if (inserted != _list.Count)
                {
                    throw new Exception("Database insert error.");
                }
                _list.Clear();

                Console.WriteLine("Pushed");
            }
        }

        /// <summary>
        /// Push buffer to database
        /// </summary>
        public void QueuePush()
        {
            if (!_list.Any())
                return;

            int inserted = _dbService.WriteBulk(_collectionName, _list);

            if (inserted != _list.Count)
            {
                throw new Exception("Database insert error.");
            }

            _logger.Debug($"BulkHelper.QueuePush inserted={inserted}");

            _list.Clear();
        }
    }
}
