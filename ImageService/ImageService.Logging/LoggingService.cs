
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
    /// <summary>
    /// Class LoggingService.
    /// </summary>
    /// <seealso cref="ImageService.Logging.ILoggingService" />
    public class LoggingService : ILoggingService
    {
        /// <summary>
        /// Delegate NotifyClientsAboutLog
        /// </summary>
        /// <param name="command">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public delegate void NotifyClientsAboutLog(CommandRecievedEventArgs command);
        public static event NotifyClientsAboutLog NotifyLogEntry;

        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // list of all the event log entries.
        /// <summary>
        /// The m log messages
        /// </summary>
        private ObservableCollection<LogEntry> m_logMessages;

        // property that wrapps logMessages.
        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>The log messages.</value>
        public ObservableCollection<LogEntry> LogMessages
        {
            get { return this.m_logMessages; }
            set { this.m_logMessages = value; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="MessageRecieved">The message recieved.</param>
        public LoggingService(EventHandler<MessageRecievedEventArgs> MessageRecieved)
        {
            this.MessageRecieved = MessageRecieved;
            this.LogMessages = new ObservableCollection<LogEntry>();
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
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
