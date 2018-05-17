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

        public ClientHandler(IImageController controller, ILoggingService log)
        {
            m_controller = controller;
            m_logging = log;
        }
        public void HandleClient(TcpClient client, NetworkStream stream, BinaryReader reader1, BinaryWriter writer1)
        {
            new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        BinaryReader reader = new BinaryReader(stream);
                        BinaryWriter writer = new BinaryWriter(stream);
                        bool res;
                        m_logging.Log("start listening ...", Logging.Modal.MessageTypeEnum.INFO);
                        string commandLine = reader.ReadString();
                        //while (reader.Peek() > 0)
                        //{
                        //    commandLine += reader.ReadLine();
                        //}
                        if (commandLine != null)
                        {

                            CommandRecievedEventArgs command = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(commandLine);
                          
                            if (command.CommandID.Equals((int)CommandEnum.CloseGui))
                            {
                                ///add
                                break;
                            }
                            string result = m_controller.ExecuteCommand(command.CommandID, command.Args, out res);
                          //  result.Replace(Environment.NewLine, "#");
                            ///   m_mtx.WaitOne();
                            writer.Write(result);
                            writer.Flush();
                            //  m_mtx.ReleaseMutex();
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
