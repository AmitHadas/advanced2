using ImageService.Commands;
using ImageService.Server;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    /// <summary>
    /// Class CloseGuiCommand.
    /// </summary>
    /// <seealso cref="ImageService.Commands.ICommand" />
    class CloseGuiCommand : ICommand
    {
        /// <summary>
        /// The m image server
        /// </summary>
        private ImageServer m_imageServer;
        /// <summary>
        /// Initializes a new instance of the <see cref="CloseGuiCommand"/> class.
        /// </summary>
        /// <param name="imageServer">The image server.</param>
        public CloseGuiCommand(ImageServer imageServer)
        {
            m_imageServer = imageServer;
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        public string Execute(string[] args, out bool result)
        {
            result = true;


            return "";
        }
    }
}
