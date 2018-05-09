using ImageService.Commands;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class RemoveHandlerCommand : ICommand
    {
        private ImageServer m_imageServer;
        public RemoveHandlerCommand(ImageServer server)
        {
            m_imageServer = server;
        }
        public string Execute(string[] args, out bool result)
        {
            result = true;
          //  m_imageServer.RemoveHandler(args[0]);
            //לעדכן app config
            return "";
        }
    }
}
