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
    /// <summary>
    /// Class ClientHandler.
    /// </summary>
    /// <seealso cref="ImageService.Communication.IClientHandler" />
    class ClientHandler : IClientHandler
    {
        /// <summary>
        /// The m controller
        /// </summary>
        private IImageController m_controller;
        /// <summary>
        /// The m logging
        /// </summary>
        private ILoggingService m_logging;
        /// <summary>
        /// The m MTX
        /// </summary>
        private static Mutex m_mtx = new Mutex();
        public event RemoveClientFromList RemoveClient;
        /// <summary>
        /// Delegate RemoveClientFromList
        /// </summary>
        /// <param name="clientToRemove">The client to remove.</param>
        public delegate void RemoveClientFromList(TcpClient clientToRemove);
        /// <summary>
        /// The MTX
        /// </summary>
        private Mutex mtx;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientHandler"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="log">The log.</param>
        public ClientHandler(IImageController controller, ILoggingService log)
        {
            m_controller = controller;
            m_logging = log;
            this.mtx = new Mutex();
        }
        /// <summary>
        /// Gets the MTX.
        /// </summary>
        /// <value>The MTX.</value>
        public Mutex Mtx
        {
            get { return this.mtx; }
        }

        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="stream">The stream.</param>
        public void HandleClient(TcpClient client, NetworkStream stream)
        {
            m_logging.Log("start listening to new client...", Logging.Modal.MessageTypeEnum.INFO);
            new Task(() =>
            {
                StreamReader reader = new StreamReader(stream);
                StreamWriter writer = new StreamWriter(stream);
                while (true)
                {
                    try
                    {

                        bool res;
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
                                m_logging.Log("Client disconnected", Logging.Modal.MessageTypeEnum.WARNING);
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
                                m_logging.Log("Client disconnected", Logging.Modal.MessageTypeEnum.WARNING);
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
            }).Start();
        }
    }
}
