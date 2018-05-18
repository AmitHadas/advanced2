using ImageService.Logging;
using ImageService.Logging.Modal;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class TcpServer
    {
        private ILoggingService m_logging;
        private ClientHandler m_clientHandler;
        private int m_port;
        private List<ClientInfo> m_clientsList;
        private TcpListener m_listener;

        public TcpServer(ILoggingService logging, ClientHandler clientHandler, int port)
        {
            this.m_logging = logging;
            this.m_clientHandler = clientHandler;
            m_clientHandler.RemoveClient += RemoveClientFromList;
            this.m_port = port;
            this.m_clientsList = new List<ClientInfo>();

        }

        public void Start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
                m_listener = new TcpListener(endPoint);
                m_listener.Start();

                m_logging.Log("Waiting for connections ...", MessageTypeEnum.INFO);
                Task task = new Task(() => {
                    while (true)
                    {
                        try
                        {
                            TcpClient client = m_listener.AcceptTcpClient();
                            m_logging.Log("Got new connection", MessageTypeEnum.INFO);
                            ClientInfo clientInfo = new ClientInfo(client);
                            m_clientsList.Add(clientInfo);
                            m_clientHandler.HandleClient(client, clientInfo.Stream);
                        }
                        catch (Exception e)
                        {
                            m_logging.Log("Exception occured", MessageTypeEnum.FAIL);
                            break;
                        }
                    }
                    m_logging.Log("Server stopped", MessageTypeEnum.INFO);
                });
                task.Start();
            }
            catch (Exception e)
            {
                m_logging.Log(e.ToString(), MessageTypeEnum.FAIL);
            }
        }
        public void Stop()
        {
            m_listener.Stop();
        }
        public void NotifyAllClients(CommandRecievedEventArgs e)
        {
            foreach (ClientInfo client in m_clientsList)
            {
                new Task(() => {
                    try
                    {
                        StreamWriter writer = new StreamWriter(client.Stream);
                        string command = JsonConvert.SerializeObject(e);
                        writer.WriteLine(command);
                        writer.Flush();
                    }
                    catch (Exception ex)
                    {
                        m_logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
                    }
                }).Start();

            }
        }
        public void RemoveClientFromList(TcpClient clientToRemove)
        {
            foreach (ClientInfo client in m_clientsList)
            {
                if (client.Client.Equals(clientToRemove))
                {
                    m_clientsList.Remove(client);
                }
            }

        }
    }
}