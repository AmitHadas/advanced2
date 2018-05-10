﻿using ImageService.Modal;
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
        public bool IsConnected { get; set; }


        private GuiClientSingleton()
        {
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
                     using (NetworkStream netWorkStream = this.m_client.GetStream())
                     using (StreamWriter writer = new StreamWriter(netWorkStream))
                     {

                         //sending data to server
                         Console.WriteLine($"Sending {commandToJson} to server");
                         m_mtx.WaitOne();
                         writer.Write(commandToJson);
                         writer.Flush();
                         m_mtx.ReleaseMutex();
                     }
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
                        NetworkStream stream = this.m_client.GetStream();
                        StreamReader reader = new StreamReader(stream);
                        string responseString = reader.ReadLine();
                        if (responseString == null)
                        {
                            Console.WriteLine("111");
                        }
                        Console.WriteLine("111");


                        //  {
                        Console.WriteLine($"received {responseString} from Server");
                            CommandRecievedEventArgs responseCommand =
                                JsonConvert.DeserializeObject<CommandRecievedEventArgs>(responseString);
                            this.UpdateResponse?.Invoke(responseCommand);
                      //  }                   
                    }
                } catch(Exception exp)
                {
                    Console.WriteLine(exp.ToString());
                }
            }).Start();
        }
    }
}
