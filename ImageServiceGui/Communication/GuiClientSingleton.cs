using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace ImageServiceGui.Communication
{
    class GuiClientSingleton
    {
        private TcpClient m_client;
        private bool m_isListening;
        public delegate void UpdateAfterResponse(CommandRecievedEventArgs e);
        public event UpdateAfterResponse UpdateResponse;
        private static GuiClientSingleton m_clientInstance;
        private static Mutex m_mtx = new Mutex();
        private bool m_isConnected;
        private NetworkStream stream;
        private StreamReader reader;
        private StreamWriter writer;
        private Mutex mtx;
        public bool IsConnected { get; private set; }


        private GuiClientSingleton()
        {
            this.mtx = new Mutex();
            this.IsConnected = this.Start();
        }
        public static GuiClientSingleton ClientInsatnce
        {
            get
            {
                if (m_clientInstance == null)
                {
                    m_clientInstance = new GuiClientSingleton();
                }
                return m_clientInstance;
            }
        }

        private bool Start()
        {
            try
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
                this.m_client = new TcpClient();
                this.m_client.Connect(endPoint);
                stream = m_client.GetStream();
                reader = new StreamReader(stream);
                writer = new StreamWriter(stream);
                Console.WriteLine("Client Connected");
                m_isListening = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        public void SendCommand(CommandRecievedEventArgs e)
        {
            new Task(() =>
             {
                 try
                 {
                        string commandToJson = JsonConvert.SerializeObject(e);
                         //sending data to server
                         Console.WriteLine($"Sending {commandToJson} to server");
                        mtx.WaitOne();
                         writer.WriteLine(commandToJson);
                         writer.Flush();
                        mtx.ReleaseMutex();
                 }
                 catch (Exception exception)
                 {
                     Console.WriteLine(exception.ToString());
                 }
             }).Start();
       
        }

        public void ReceivedCommand()
        {
            new Task(() =>
            {
                try
                {
                    while(m_isListening)
                    {
                        //Thread.Sleep(1000);
                        //mtx.WaitOne();
                        string responseString = reader.ReadLine();
                        while (reader.Peek() > 0)
                        {
                            responseString += reader.ReadLine();
                        }
                        //mtx.ReleaseMutex();
                        if (responseString != "")
                        {
                        //    responseString.Replace("#", Environment.NewLine);
                            Console.WriteLine($"received {responseString} from Server");
                            CommandRecievedEventArgs responseCommand =
                                    JsonConvert.DeserializeObject<CommandRecievedEventArgs>(responseString);
                            this.UpdateResponse?.Invoke(responseCommand);
                        }           
                    }
                } catch(Exception exp)
                {
                    Console.WriteLine(exp.ToString());
                }
            }).Start();
        }
    }
}
