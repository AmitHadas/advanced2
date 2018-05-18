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
using System.Threading;
using ImageService.Logging;

namespace ImageService.Communication
{
    class ClientHandler : IClientHandler
    {
        private IImageController m_controller;
        private ILoggingService m_logging;
        private static Mutex m_mtx = new Mutex();
        public event RemoveClientFromList RemoveClient;
        public delegate void RemoveClientFromList(TcpClient clientToRemove);

        public ClientHandler(IImageController controller, ILoggingService log)
        {
            m_controller = controller;
            m_logging = log;
        }
        public void HandleClient(TcpClient client, NetworkStream stream)
        {
            new Task(() =>
            {
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                while (true)
                {
                    try
                    {

                        bool res;
                        m_logging.Log("start listening ...", Logging.Modal.MessageTypeEnum.INFO);
                        string commandLine = reader.ReadLine();
                        while (reader.Peek() > 0)
                        {
                            commandLine += reader.ReadLine();
                        }
                        if (commandLine != null)
                        {

                            CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                          
                            if (command.CommandID.Equals((int)CommandEnum.CloseGui))
                            {
                                ///add
                                break;
                            }
                            string result = m_controller.ExecuteCommand(command.CommandID, command.Args, out res);
                            try
                            {
                                writer.WriteLine(result);
                                writer.Flush();
                            } catch(Exception e)
                            {
                                RemoveClient?.Invoke(client);
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return;
                    }

                }
                // client.Close();
            }).Start();
        }
    }
}
