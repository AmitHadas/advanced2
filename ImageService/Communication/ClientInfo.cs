using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    /// <summary>
    /// Class ClientInfo.
    /// </summary>
    class ClientInfo
    {
        /// <summary>
        /// The m client
        /// </summary>
        private TcpClient m_client;
        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public TcpClient Client
        {
            get { return m_client; }
            set { m_client = value; }
        }
        /// <summary>
        /// The m stream
        /// </summary>
        private NetworkStream m_stream;
        /// <summary>
        /// Gets or sets the stream.
        /// </summary>
        /// <value>The stream.</value>
        public NetworkStream Stream
        {
            get { return m_stream; }
            set { m_stream = value; }
        }
       
        /// <summary>
        /// Initializes a new instance of the <see cref="ClientInfo"/> class.
        /// </summary>
        /// <param name="client">The client.</param>
        public ClientInfo(TcpClient client)
        {
            this.m_client = client;
            this.m_stream = client.GetStream();
            //this.m_reader = new BinaryReader(m_stream);
            //this.m_writer = new BinaryWriter(m_stream);
        }
    }
}
