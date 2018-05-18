
using ImageService.Infrastructure.Enums;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public delegate void NotifyClientsAboutLog(CommandRecievedEventArgs command);
        public static event NotifyClientsAboutLog NotifyLogEntry;

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // list of all the event log entries.
        private ObservableCollection<LogEntry> m_logMessages;

        // property that wrapps logMessages.
        public ObservableCollection<LogEntry> LogMessages
        {
            get { return this.m_logMessages; }
            set { this.m_logMessages = value; }
        }


        public LoggingService(EventHandler<MessageRecievedEventArgs> MessageRecieved)
        {
            this.MessageRecieved = MessageRecieved;
            this.LogMessages = new ObservableCollection<LogEntry>();
        }

        public void Log(string message, MessageTypeEnum type)
        {
            LogEntry logEntry = new LogEntry(Enum.GetName(typeof(MessageTypeEnum), type), message);
            this.LogMessages.Add(logEntry);
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
            string logJson = JsonConvert.SerializeObject(logEntry);
            string[] args = { logJson };
            CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.LogCommand, args, "");
            NotifyLogEntry?.Invoke(command);

        }
    }
}
