﻿
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
   
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        public LoggingService(EventHandler<MessageRecievedEventArgs> MessageRecieved)
        {
            ///       this.MessageRecieved = new EventHandler<MessageRecievedEventArgs>();
            this.MessageRecieved = MessageRecieved;
        }

        public void Log(string message, MessageTypeEnum type)
        {
            ///לזכור לטפל בזה
             MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(message, type));
        }
    }
}
