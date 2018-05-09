using ImageService.Controller;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
        private IImageController m_controller;

        public ClientHandler(IImageController controller)
        {
            m_controller = controller;
        }
        public void HandleClient(TcpClient client)
        {
            new Task(() =>
            {
                bool res;
                using (NetworkStream stream = client.GetStream())
                using (StreamReader reader = new StreamReader(stream))
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string commandLine = "";
                //    while (reader.Peek() > 0)
                //    {
                        commandLine = reader.ReadLine();
               //     }
                    if(commandLine != null)
                    {
                        CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                        if(command.CommandID.Equals((int)CommandEnum.CloseGui))
                        {
                       ///add
                        }
                        string result = m_controller.ExecuteCommand(command.CommandID, command.Args, out res);
                        writer.Write(result);
                        writer.Flush();
                    }
                    
                    
                }
                client.Close();
            }).Start();
        }
    }
}
