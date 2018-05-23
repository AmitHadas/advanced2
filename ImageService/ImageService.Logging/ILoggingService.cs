using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// Interface ILoggingService
    /// </summary>
    public interface ILoggingService
    {
        // property that wrapps logMessages.
        /// <summary>
        /// Gets or sets the log messages.
        /// </summary>
        /// <value>The log messages.</value>
        ObservableCollection<LogEntry> LogMessages
        {
            get; set;
        }
        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        // Logging the Message
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        void Log(string message, MessageTypeEnum type);
    }
}
