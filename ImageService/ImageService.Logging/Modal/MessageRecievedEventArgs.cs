using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// Class MessageRecievedEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class MessageRecievedEventArgs : EventArgs
    {

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
        //constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRecievedEventArgs"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        public MessageRecievedEventArgs(string message, MessageTypeEnum type)
        {
            this.Message = message;
            this.Status = type;

        }
    }
}
