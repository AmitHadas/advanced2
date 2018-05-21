using ImageService.Commands;
using ImageService.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    class CloseGuiCommand : ICommand
    {
        private ImageServer m_imageServer;
        public CloseGuiCommand(ImageServer imageServer)
        {
            m_imageServer = imageServer;
        }
        
        public string Execute(string[] args, out bool result)
        {
            result = true;


            return "";
        }
    }
}
