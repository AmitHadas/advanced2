using ImageServiceWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace ImageServiceWeb.Controllers
{
    public class ProductsController : Controller
    {
        static string[] s1 = ReadFromFile(1);
        static string[] s2 = ReadFromFile(2);
        static bool isServiceRunning = false;
        static int numOfPictures = 100;
        static List<Student> students = new List<Student>()
        {
          new Student  { FirstName = s1[0] , LastName =  s1[1], ID = s1[2] },
          new Student  { FirstName = s2[0] , LastName =  s2[1], ID = s2[2] }
        };

        // GET: Products
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ImageWeb()
        {
            Data data = new Data();
            data.students = students;
            data.numOfPictures = numOfPictures;
            data.isServiceRunning = isServiceRunning;

            return View(students);
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
            public bool isServiceRunning;
            public int numOfPictures;
        }
    }
}