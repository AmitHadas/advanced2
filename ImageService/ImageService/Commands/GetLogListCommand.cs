using ImageService.Commands;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
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
            string jsonLogMessages = JsonConvert.SerializeObject(logs);
            string[] arr = new string[1];
            arr[0] = jsonLogMessages;
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.GetLogList, arr, "");
            return JsonConvert.SerializeObject(command);

        }
    }
}
