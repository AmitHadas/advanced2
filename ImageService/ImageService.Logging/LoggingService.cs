
using ImageService.Logging.Modal;
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
   
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // list of all the event log entries.
        private ObservableCollection<LogEntry> m_logMessages;

        // property that wrapps logMessages.
        public ObservableCollection<LogEntry> LogMessages
        {
            get { return this.m_logMessages; }
            //set { throw new NotImplementedException(); }
            set { this.m_logMessages = value; }
        } 


        public LoggingService(EventHandler<MessageRecievedEventArgs> MessageRecieved)
        {
            this.MessageRecieved = MessageRecieved;
            this.LogMessages = new ObservableCollection<LogEntry>();
        }
 
        public void Log(string message, MessageTypeEnum type)
        {
            this.LogMessages.Add(new LogEntry(Enum.GetName(typeof(MessageTypeEnum), type), message));
             MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));

        }
    }
}
