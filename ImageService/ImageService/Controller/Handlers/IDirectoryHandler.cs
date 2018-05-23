using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// Interface IDirectoryHandler
    /// </summary>
    public interface IDirectoryHandler
    {

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        string Path
        {
            get; set;
        }
        // The Event That Notifies that the Directory is being closed
        event EventHandler<DirectoryCloseEventArgs> DirectoryClose;
        // The Function Recieves the directory to Handle
        /// <summary>
        /// Starts the handle directory.
        /// </summary>
        /// <param name="dirPath">The dir path.</param>
        void StartHandleDirectory(string dirPath);
        // The Event that will be activated upon new Command
        /// <summary>
        /// Handles the <see cref="E:CommandRecieved" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        void OnCommandRecieved(object sender, CommandRecievedEventArgs e);
      
    }
}
