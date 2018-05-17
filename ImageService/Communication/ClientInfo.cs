using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    class ClientInfo
    {
        private TcpClient m_client;
        public TcpClient Client
        {
            get { return m_client; }
            set { m_client = value; }
        }
        private NetworkStream m_stream;
        public NetworkStream Stream
        {
            get { return m_stream; }
            set { m_stream = value; }
        }
        //private BinaryReader m_reader;
        //public BinaryReader Reader
        //{
        //    get { return m_reader; }
        //    set { m_reader = value; }
        //}
        //private BinaryWriter m_writer;
        //public BinaryWriter Writer
        //{
        //    get { return m_writer; }
        //    set { m_writer = value; }
        //}
        public ClientInfo(TcpClient client)
        {
            this.m_client = client;
            this.m_stream = client.GetStream();
            //this.m_reader = new BinaryReader(m_stream);
            //this.m_writer = new BinaryWriter(m_stream);
        }
    }
}
