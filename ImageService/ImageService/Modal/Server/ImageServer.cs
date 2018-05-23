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
    /// <summary>
    /// Class ImageServer.
    /// </summary>
    public class ImageServer
    {
        /// <summary>
        /// The m controller
        /// </summary>
        private IImageController m_controller;
        /// <summary>
        /// The m logging
        /// </summary>
        private ILoggingService m_logging;
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;
        /// <summary>
        /// The handlers list
        /// </summary>
        private LinkedList<IDirectoryHandler> handlersList;
        /// <summary>
        /// Delegate NotifyClients
        /// </summary>
        /// <param name="command">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public delegate void NotifyClients(CommandRecievedEventArgs command);
        public static event NotifyClients NotifyHandlerRemoved;
        public static event NotifyClients NotifyCloseGui;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageServer"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="logging">The logging.</param>
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
        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="eventArgs">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public void sendCommand(CommandRecievedEventArgs eventArgs)
        {
            CommandRecieved?.Invoke(this, eventArgs); //– closes handlers  
        }

        /// <summary>
        /// Removes the directory handler.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="e">The <see cref="DirectoryCloseEventArgs"/> instance containing the event data.</param>
        public void RemoveDirectoryHandler(string path, DirectoryCloseEventArgs e)
        {
            for(int i = 0; i < handlersList.Count; i++)
            {
                if(handlersList.ElementAt<IDirectoryHandler>(i).Path == path)
                {
                    onClose(handlersList.ElementAt<IDirectoryHandler>(i), e);
                }
            }
        }

        //The function removes the functions from the events and notify the logger.
        /// <summary>
        /// Ons the close.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DirectoryCloseEventArgs"/> instance containing the event data.</param>
        public void onClose(object sender, DirectoryCloseEventArgs e)
        {
            if (sender is DirectoryHandler)
            {
                DirectoryHandler handler = (DirectoryHandler)sender;
                CommandRecieved -= handler.OnCommandRecieved;
                handler.DirectoryClose -= onClose;
                handler.OnClose();
                m_logging.Log(e.Message, Logging.Modal.MessageTypeEnum.INFO);
            }
        }

        /// <summary>
        /// Removes the handler.
        /// </summary>
        /// <param name="handlerName">Name of the handler.</param>
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

        /// <summary>
        /// Removes the handler event.
        /// </summary>
        /// <param name="commandRecievedEventArgs">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public static void RemoveHandlerEvent(CommandRecievedEventArgs commandRecievedEventArgs)
        {
            NotifyHandlerRemoved?.Invoke(commandRecievedEventArgs);
        }

        /// <summary>
        /// Closes the GUI event.
        /// </summary>
        /// <param name="command">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public static void CloseGuiEvent(CommandRecievedEventArgs command)
        {
            NotifyCloseGui?.Invoke(command);
        }
    }
}
