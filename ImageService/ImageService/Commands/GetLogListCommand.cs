using ImageService.Commands;
using ImageService.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class GetLogListCommand : ICommand
    {
        private ILoggingService m_log;
        public GetLogListCommand(ILoggingService log)
        {
            this.m_log = log;
        }
        public string Execute(string[] args, out bool result)
        {
            result = true;
            ObservableCollection<LogEntry> logs = m_log.LogMessages;
            string logStr = JsonConvert.SerializeObject(logs);
            return logStr;
        }
    }
}
