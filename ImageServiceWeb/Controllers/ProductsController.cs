using ImageServiceWeb.Models;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using ImageServiceWeb.Communication;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.Collections.ObjectModel;
using System.Threading;
using System.IO;


namespace ImageServiceWeb.Controllers
{
    public class ProductsController : Controller
    {
        

        static ClientSingleton client = ClientSingleton.ClientInsatnce;
        static string[] s1 = ReadFromFile(1);
        static string[] s2 = ReadFromFile(2);
        static string m_isServiceRunning = client.IsConnectedStr;
        static int m_numOfPictures = 100;
        static List<Student> m_students = new List<Student>()
        {
          new Student  { FirstName = s1[0] , LastName =  s1[1], ID = s1[2] },
          new Student  { FirstName = s2[0] , LastName =  s2[1], ID = s2[2] }
        };
        static AppConfig config;

        public struct AppConfig
        {
            public string OutputDir;
            public string SourceName;
            public string LogName;
            public string ThumbSize;
            public ObservableCollection<string> handlers;
            public bool isReady;
        }

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImageWeb()
        {
           
            //Thread.Sleep(1000);
            if (!config.isReady) {
                LoadConfig();
            }
            Data data = new Data();
            data.students = m_students;
            data.numOfPictures = CountPictures();
            data.isServiceRunning = m_isServiceRunning;

            return View(data);
        }
        public static string[] ReadFromFile(int line)
        {
            string[] lines = System.IO.File.ReadAllLines(HostingEnvironment.MapPath("~/App_Data/StudentsDetails.txt"));
            string requiredLine = lines[line - 1];
            string[] words = requiredLine.Split(' ');
            return words;
        }

        public struct Data
        {
            public List<Student> students;
            public string isServiceRunning;
            public int numOfPictures;
        }

        public ActionResult Config()
        {
            if (!config.isReady)
            {
                LoadConfig();
            }
            return View(config);
        }

        private void LoadConfig()
        {
            while(client == null) { }
            client.UpdateResponse += HandleCommand;
            client.ReceivedCommand();
            string[] args = { };
            client.SendCommand(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, args, ""));
        }

        public ActionResult DeleteHandler(string handlerToRemove)
        {
            for (int i = 0; i < config.handlers.Count; i++)
            {
                if (config.handlers[i].Equals(handlerToRemove))
                {
                    config.handlers.RemoveAt(i);

                    string updatedList = getUpdatedList();
                    string[] args = { updatedList, handlerToRemove };
                    client.SendCommand(new CommandRecievedEventArgs((int)CommandEnum.CloseHandler, args, ""));
                }
            }
            return View();
        }


        //public void RemoveHandlerFromList(string[] args)
        //{
        //    string[] directories = args[0].Split(';');
        //    foreach (var dir in config.handlers)
        //    {
        //        if (!directories.Contains(dir))
        //        {
        //            //App.Current.Dispatcher.Invoke(delegate
        //           // {
        //                config.handlers.Remove(dir);
        //            //});
        //        }
        //    }
        //}

        private string getUpdatedList()
        {
            string handlers = "";
            for (int i = 0; i < config.handlers.Count; i++)
            {
                if (i != 0)
                {
                    handlers += ";";
                }
                handlers += config.handlers[i];
            }
            return handlers;
        }

        public void HandleCommand(CommandRecievedEventArgs e)
        {
            if (e.CommandID == (int)CommandEnum.GetConfigCommand)
            {
                string[] args = e.Args;
                config = new AppConfig();

                config.OutputDir = args[0];
                config.SourceName = args[1];
                config.LogName = args[2];
                config.ThumbSize = args[3];

                string[] directories = args[4].Split(';');
                config.handlers = new ObservableCollection<string>();
                foreach (var dir in directories)
                {
                    config.handlers.Add(dir);
                }
                config.isReady = true;
            } else if (e.CommandID == (int)CommandEnum.CloseHandler)
            {
                //RemoveHandlerFromList(e.Args);
            }
        }

        public int CountPictures()
        {
            string outputDirPath = config.OutputDir;
            //Get Files in Specific directory
            string[] directoryFiles = Directory.GetFiles(outputDirPath, "*", SearchOption.AllDirectories);
            //initialize counter
            int counter = 0;
            //loop on file paths
            foreach (string filePath in directoryFiles)
            {
                //check files extensions
                if (Path.GetExtension(filePath) == ".jpg" || Path.GetExtension(filePath) == ".png" ||
                    Path.GetExtension(filePath) == ".bmp" || Path.GetExtension(filePath) == ".gif")
                {
                    counter++;
                }
            }
            return counter;
        }
    }
}