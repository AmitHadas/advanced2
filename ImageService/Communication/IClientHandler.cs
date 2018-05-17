﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Communication
{
    interface IClientHandler
    {
        void HandleClient(TcpClient client, NetworkStream stream, StreamReader reader, StreamWriter writer);
    }
}
