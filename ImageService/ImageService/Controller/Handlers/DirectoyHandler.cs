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
    public class DirectoryHandler : IDirectoryHandler
    {
        // The Image Processing Controller
        private IImageController m_controller;
        private ILoggingService m_logging;
        // The Watcher of the Dir
        private FileSystemWatcher m_dirWatcher;
        // The Path of directory
        private string m_path;
        public string Path
        {
            get; set;
        }
        private ICollection<string> extensions;
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;              // The Event That Notifies that the Directory is being closed

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

        public void StartHandleDirectory(string dirPath)
        {
            this.m_dirWatcher = new FileSystemWatcher();
            try
            {
                m_dirWatcher.Path = dirPath;
                this.m_path = dirPath;
            } catch(Exception e) { }
            // Add event handlers.
            //m_dirWatcher.Changed += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.Created += new FileSystemEventHandler(OnCreated);

            // Begin watching.
           // m_dirWatcher.EnableRaisingEvents = true;
        }

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
    }
}
