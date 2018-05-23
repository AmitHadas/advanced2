using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    /// <summary>
    /// Interface IImageController
    /// </summary>
    public interface IImageController
    {
        //   ImageServer ImageServerProp { get; set; }
        //The function get command id and execute the specific command by dictionary.
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="commandID">The command identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        string ExecuteCommand(int commandID, string[] args, out bool result);          // Executing the Command Requet

        /// <summary>
        /// Sets the server.
        /// </summary>
        /// <param name="s">The s.</param>
        void setServer(ImageServer s);
    }
}
