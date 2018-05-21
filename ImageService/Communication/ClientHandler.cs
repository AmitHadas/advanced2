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
        private Mutex mtx;
    
        public ClientHandler(IImageController controller, ILoggingService log)
        {
            m_controller = controller;
            m_logging = log;
            this.mtx = new Mutex();
        }
        public Mutex Mtx
        {
            get { return this.mtx; }
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
                                string[] args = { JsonConvert.SerializeObject(client) };
                                CommandRecievedEventArgs closeCommand = new CommandRecievedEventArgs((int)CommandEnum.CloseGui, args, "");
                                m_controller.ExecuteCommand(closeCommand.CommandID, closeCommand.Args, out res);
                                m_logging.Log("Client disconnected", Logging.Modal.MessageTypeEnum.INFO);
                                break;
                            }
                            string result = m_controller.ExecuteCommand(command.CommandID, command.Args, out res);
                            try
                            {
                              //  Thread.Sleep(1000);
                                mtx.WaitOne();
                                writer.WriteLine(result);
                                writer.Flush();
                                mtx.ReleaseMutex();
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
