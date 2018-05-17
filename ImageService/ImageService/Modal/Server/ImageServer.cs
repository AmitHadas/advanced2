using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Logging.Modal;

namespace ImageService.Server
{
    public class ImageServer
    {
        private IImageController m_controller;
        private ILoggingService m_logging;
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; 
        private LinkedList<IDirectoryHandler> handlersList;
        public delegate void NotifyClients(CommandRecievedEventArgs command);
        public static event NotifyClients NotifyHandlerRemoved;

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.handlersList = new LinkedList<IDirectoryHandler>();

            string sourceDir = ConfigurationManager.AppSettings.Get("Handler");
            string[] directories = sourceDir.Split(';');
            foreach (var dir in directories)
            {
                IDirectoryHandler handler = new DirectoryHandler(this.m_controller, this.m_logging);
                this.handlersList.AddLast(handler);
                // start listen to the directory
                handler.StartHandleDirectory(dir);
                this.m_logging.Log("handler for " + dir + " was created", Logging.Modal.MessageTypeEnum.INFO);
                CommandRecieved += handler.OnCommandRecieved;
                handler.DirectoryClose += onClose;
            }
        }

        //The function gets command args and invoke the event. 
        public void sendCommand(CommandRecievedEventArgs eventArgs)
        {
            CommandRecieved?.Invoke(this, eventArgs); //– closes handlers  
        }

        //The function removes the functions from the events and notify the logger.
        public void onClose(object sender, DirectoryCloseEventArgs e)
        {
            if (sender is DirectoryHandler)
            {
                DirectoryHandler handler = (DirectoryHandler)sender;
                CommandRecieved -= handler.OnCommandRecieved;
                handler.DirectoryClose -= onClose;
                m_logging.Log(e.Message, Logging.Modal.MessageTypeEnum.INFO);
            }
        }

        public void RemoveHandler(string handlerName)
        {
            foreach (IDirectoryHandler handler in handlersList)
            {
                if (handler.Path.Equals(handlerName))
                {
                    string[] args = { handler.Path };
                    handlersList.Remove(handler);
                    //NotifyHandlerRemoved?.Invoke(new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, null));
                    m_logging.Log("Remove handler " + handlerName, MessageTypeEnum.INFO);

                }
            }
        }

        public static void PerformEvent(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            NotifyHandlerRemoved.Invoke(commandRecievedEventArgs);
        }
    }
}
