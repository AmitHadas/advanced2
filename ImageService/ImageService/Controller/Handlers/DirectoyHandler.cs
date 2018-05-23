using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;
using ImageService.Controller.Handlers;



namespace ImageService.Controller.Handlers
{
    /// <summary>
    /// Class DirectoryHandler.
    /// </summary>
    /// <seealso cref="ImageService.Controller.Handlers.IDirectoryHandler" />
    public class DirectoryHandler : IDirectoryHandler
    {
        // The Image Processing Controller
        /// <summary>
        /// The m controller
        /// </summary>
        private IImageController m_controller;
        /// <summary>
        /// The m logging
        /// </summary>
        private ILoggingService m_logging;
        // The Watcher of the Dir
        /// <summary>
        /// The m dir watcher
        /// </summary>
        private FileSystemWatcher m_dirWatcher;
        // The Path of directory
        /// <summary>
        /// The m path
        /// </summary>
        private string m_path;
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get
            {
                return this.m_path;
            }
            set { this.m_path = value; }
        }
        /// <summary>
        /// The extensions
        /// </summary>
        private ICollection<string> extensions;
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryHandler"/> class.
        /// </summary>
        /// <param name="controller">The controller.</param>
        /// <param name="logging">The logging.</param>
        public DirectoryHandler(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
            this.extensions = new LinkedList<string>();
            this.extensions.Add(".jpg");
            this.extensions.Add(".bmp");
            this.extensions.Add(".gif");
            this.extensions.Add(".png");
        }

        /// <summary>
        /// Starts the handle directory.
        /// </summary>
        /// <param name="dirPath">The dir path.</param>
        public void StartHandleDirectory(string dirPath)
        {
            this.m_dirWatcher = new FileSystemWatcher();
            try
            {
                m_dirWatcher.Path = dirPath;
                this.m_path = dirPath;
            } catch(Exception e) { }
            // Add event handlers.
            m_dirWatcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
            m_dirWatcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Handles the <see cref="E:Created" /> event.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            //  bool isSuccess;
            string filePath = e.FullPath;
            string extension = System.IO.Path.GetExtension(filePath);
            if (this.extensions.Contains(extension))
            {
                string[] filePath2 = new string[1];
                filePath2[0] = filePath;
                //this.m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, filePath2, out isSuccess);
                this.OnCommandRecieved(this, new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, filePath2, null));
            }
        }


        /// <summary>
        /// Handles the <see cref="E:CommandRecieved" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="CommandRecievedEventArgs"/> instance containing the event data.</param>
        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                try
                {
                    this.m_dirWatcher.EnableRaisingEvents = false;
                 //   this.m_dirWatcher.Created -= new FileSystemEventHandler(OnCreated);
                    m_dirWatcher.Dispose();
                    DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path, "close handler at path " + m_path));

                    this.m_logging.Log("handler of " + this.m_path + " was closed", MessageTypeEnum.INFO);
                }
                catch (Exception exception)
                {
                    this.m_logging.Log(exception.Data.ToString(), MessageTypeEnum.FAIL);
                }
            }
            else if (e.CommandID == (int)CommandEnum.NewFileCommand)
            {
                bool result;
                string resultOfCommand = m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, e.Args, out result);
            }
        }

        /// <summary>
        /// Raises the Close event.
        /// </summary>
        public void OnClose()
        {
            try
            {
                this.m_dirWatcher.EnableRaisingEvents = false;
                //   this.m_dirWatcher.Created -= new FileSystemEventHandler(OnCreated);
                m_dirWatcher.Dispose();
                DirectoryClose?.Invoke(this, new DirectoryCloseEventArgs(m_path, "close handler at path " + m_path));
                this.m_logging.Log("handler of " + this.m_path + " was closed", MessageTypeEnum.INFO);
            }
            catch (Exception exception)
            {
                this.m_logging.Log(exception.Data.ToString(), MessageTypeEnum.FAIL);
            }
        }
    }
}
