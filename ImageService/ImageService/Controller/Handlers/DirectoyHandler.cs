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
            m_dirWatcher.Path = dirPath;
            this.m_path = dirPath;
          //   m_dirWatcher.Filter = "*.jpg *.gif *.bmp *.png";
           // m_dirWatcher.Filter = "*.jpg";

            // Add event handlers.
            m_dirWatcher.Changed += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.Created += new FileSystemEventHandler(OnChanged);
            m_dirWatcher.Deleted += new FileSystemEventHandler(OnChanged);

            // Begin watching.
            m_dirWatcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            bool isSuccess;
            string filePath = e.FullPath;
            string extension = Path.GetExtension(filePath);
            if (this.extensions.Contains(extension))
            {
                string[] filePath2 = new string[1];
                filePath2[0] = filePath;
                this.m_controller.ExecuteCommand((int)CommandEnum.NewFileCommand, filePath2, out isSuccess);
            }
        }


        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            //bool isSuccess;
            //string filePath = e.FullPath();
            //this.m_controller.executeCommand(CommandEnum.NewFileCommand, filePath, out isSuccess);

            //Image myImage = Image.FromFile(filePath);
            //PropertyItem propItem = myImage.GetPropertyItem(306);
            //DateTime dateTime;

            ////Convert date taken metadata to a DateTime object
            //string sdate = Encoding.UTF8.GetString(propItem.Value).Trim();
            //string secondhalf = sdate.Substring(sdate.IndexOf(" "), (sdate.Length - sdate.IndexOf(" ")));
            //string firsthalf = sdate.Substring(0, 10);
            //firsthalf = firsthalf.Replace(":", "-");
            //sdate = firsthalf + secondhalf;
            //dateTime = DateTime.Parse(sdate);

            //string year = dateTime.Year;
            //string yearPath = this.m_path + "/" + year;
            //string monthPath = this.m_path + "/" + yaer + dateTime.Month;
            //if (!Directory.Exists(yearPath))
            //{
            //    Directory.CreateDirectory(yearPath);
            //    Directory.CreateDirectory(monthPath);
            //}
            //else if (!Directory.Exists(monthPath))
            //{
            //    Directory.CreateDirectory(monthPath);
            //}
            //System.IO.File.Move(filePath, monthPath);
        }
        
    }
}
