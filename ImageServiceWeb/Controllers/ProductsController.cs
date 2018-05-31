using ImageServiceWeb.Models;
using System.Collections.Generic;
using System.Web.Hosting;
using System.Web.Mvc;
using ImageServiceWeb.Communication;
using ImageService.Modal;
using ImageService.Infrastructure.Enums;
using System.Collections.ObjectModel;
using System.Threading;

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
            Data data = new Data();
            data.students = m_students;
            data.numOfPictures = m_numOfPictures;
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
            client.UpdateResponse += GetConfig;
            client.ReceivedCommand();
            string[] args = { };
            client.SendCommand(new CommandRecievedEventArgs((int)CommandEnum.GetConfigCommand, args, ""));
            Thread.Sleep(1000);
            return View(config);
        }

        public void GetConfig(CommandRecievedEventArgs e)
        {
               if(e.CommandID == (int)CommandEnum.GetConfigCommand)
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
            }
        }
    }
}