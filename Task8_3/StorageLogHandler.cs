﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task7
{
    internal class StorageLogHandler
    {
        private List<string> _logs;
        public string this[int index]
        {
            get { return _logs[index]; }
        }
        public int Length => _logs.Count;
        public StorageLogHandler()
        {
            _logs = new List<string>(); 
        }
        public StorageLogHandler(IEnumerable<string> logs)
        {
            _logs = new List<string>(logs);
        }
        public StorageLogHandler(string logFilePath)
        {
            _logs = new List<string>();
            try
            {
                _logs = File.ReadAllLines(logFilePath).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<string> GetLogsAfterDate(DateTime startDate)
        {

            return _logs.Where(x =>
            {
                var tmpSlpitedLine = x.Trim().Split(' ',StringSplitOptions.RemoveEmptyEntries);
                var time = DateTime.Parse(tmpSlpitedLine[^2] + " " + tmpSlpitedLine[^1]);
                return time > startDate;
            });
        }


    }
}
