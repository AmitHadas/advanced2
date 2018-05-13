using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public interface ILoggingService
    {
        // property that wrapps logMessages.
        ObservableCollection<LogEntry> LogMessages
        {
            get; set;
        }
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // Logging the Message
        void Log(string message, MessageTypeEnum type);
    }
}
