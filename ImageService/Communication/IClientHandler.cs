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
    /// Interface IClientHandler
    /// </summary>
    interface IClientHandler
    {
        /// <summary>
        /// Handles the client.
        /// </summary>
        /// <param name="client">The client.</param>
        /// <param name="stream">The stream.</param>
        void HandleClient(TcpClient client, NetworkStream stream);
    }
}
