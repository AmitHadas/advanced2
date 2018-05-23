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
    /// <summary>
    /// Class GetLogListCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class GetLogListCommand : ICommand
    {
        /// <summary>
        /// The m log
        /// </summary>
        private ILoggingService m_log;
        /// <summary>
        /// Initializes a new instance of the <see cref="GetLogListCommand"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public GetLogListCommand(ILoggingService log)
        {
            this.m_log = log;
        }
        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
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
