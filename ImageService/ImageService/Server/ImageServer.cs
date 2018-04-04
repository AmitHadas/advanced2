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

namespace ImageService.Server
{
    public class ImageServer
    {
        private IImageController m_controller;
        private ILoggingService m_logging;
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved; 
        private LinkedList<IDirectoryHandler> handlersList;  
       
        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.handlersList = new LinkedList<IDirectoryHandler>();

            string sourceDir = ConfigurationManager.AppSettings.Get("Handler");
            string[] directories = sourceDir.Split(';');
            foreach(var dir in directories)
          {
                IDirectoryHandler handler = new DirectoryHandler(this.m_controller, this.m_logging);
                this.handlersList.AddLast(handler);
                // start listen to the directory
               handler.StartHandleDirectory(dir);
                this.m_logging.Log("handler for " + dir + " was created", Logging.Modal.MessageTypeEnum.INFO);
          }
        }

        public void onClose()
        {
            foreach(var handler in this.handlersList)
            {
                handler.OnCommandRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, null));
            }
        }
    }
}
