using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks;
using ImageService.Modal;
using Newtonsoft.Json;


namespace ImageServiceWeb.Communication
{
    class ClientSingleton
    {



        private TcpClient m_client;
        /// <summary>
        /// The m is listening
        /// </summary>
        private bool m_isListening;
        /// <summary>
        /// Delegate UpdateAfterResponse
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public delegate void UpdateAfterResponse(CommandRecievedEventArgs e);
        public event UpdateAfterResponse UpdateResponse;
        /// <summary>
        /// The m client instance
        /// </summary>
        private static ClientSingleton m_clientInstance;
        /// <summary>
        /// The m MTX
        /// </summary>
        private static Mutex m_mtx = new Mutex();
        /// <summary>
        /// The m is connected
        /// </summary>
        private bool m_isConnected;
        /// <summary>
        /// The stream
        /// </summary>
        /// 

        private string m_isConnectedStr;

        private NetworkStream stream;
        /// <summary>
        /// The reader
        /// </summary>
        private StreamReader reader;
        /// <summary>
        /// The writer
        /// </summary>
        private StreamWriter writer;
        /// <summary>
        /// The MTX
        /// </summary>
        private Mutex mtx;
        /// <summary>
        /// Gets a value indicating whether this instance is connected.
        /// </summary>
        /// <value><c>true</c> if this instance is connected; otherwise, <c>false</c>.</value>
        /// 

        public string IsConnectedStr
        {
            get
            {
                if (IsConnected)
                {
                    return "yes";
                }
                return "no";
            }
            set { }
        }
        public bool IsConnected { get; private set; }

        /// <summary>
        /// Prevents a default instance of the <see cref="GuiClientSingleton"/> class from being created.
        /// </summary>
        private ClientSingleton()
        {
            this.mtx = new Mutex();
            this.IsConnected = this.Start();
        }
        /// <summary>
        /// Gets the client insatnce.
        /// </summary>
        /// <value>The client insatnce.</value>
        public static ClientSingleton ClientInsatnce
        {
            get
            {
                if (m_clientInstance == null)
                {
                    m_clientInstance = new ClientSingleton();
                }
                return m_clientInstance;
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
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
                m_isListening = false;
                Console.WriteLine(e.ToString());
                return false;
            }
        }

        /// <summary>
        /// Sends the command.
        /// </summary>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
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

        /// <summary>
        /// Receiveds the command.
        /// </summary>
        public void ReceivedCommand()
        {
            new Task(() =>
            {
                try
                {
                    while (m_isListening)
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
                }
                catch (Exception exp)
                {
                    Console.WriteLine(exp.ToString());
                }
            }).Start();
        }

    }
}




