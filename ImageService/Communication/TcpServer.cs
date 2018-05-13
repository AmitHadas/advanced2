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
        private List<TcpClient> m_clientsList;
        private TcpListener m_listener;

        public TcpServer(ILoggingService logging, ClientHandler clientHandler, int port)
        {
            this.m_logging = logging;
            this.m_clientHandler = clientHandler;
            this.m_port = port;
            this.m_clientsList = new List<TcpClient>();

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
                            m_clientsList.Add(client);
                            m_clientHandler.HandleClient(client, client.GetStream());
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
            foreach (TcpClient client in m_clientsList)
            {
                new Task(() => {
                    try
                    {
                        NetworkStream stream = client.GetStream();
                        StreamWriter writer = new StreamWriter(stream);
                        string command = JsonConvert.SerializeObject(e);
                        writer.Write(command);
                    }
                    catch (Exception ex)
                    {
                        m_logging.Log(ex.ToString(), MessageTypeEnum.FAIL);
                    }
                }).Start();

            }
        }
    }
}