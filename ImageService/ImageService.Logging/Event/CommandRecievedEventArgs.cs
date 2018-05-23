using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Modal
{
    /// <summary>
    /// Class CommandRecievedEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class CommandRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public int CommandID { get; set; }      // The Command ID
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string[] Args { get; set; }
        /// <summary>
        /// Gets or sets the request dir path.
        /// </summary>
        /// <value>The request dir path.</value>
        public string RequestDirPath { get; set; }  // The Request Directory
        //constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRecievedEventArgs"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="path">The path.</param>
        public CommandRecievedEventArgs(int id, string[] args, string path)
        {
            CommandID = id;
            Args = args;
            RequestDirPath = path;
        }
    }
}
