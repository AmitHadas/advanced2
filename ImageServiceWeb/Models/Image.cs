using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class Image
    {
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }

        public Image(string path)
        {
            this.ImagePath = path;
            this.ImageName = Path.GetFileName(path);
            DateTime dateTime = getImageDate(path);
            this.Year = dateTime.Year.ToString();
            this.Month = dateTime.Month.ToString();
        }

        public DateTime getImageDate(string path)
        {
            DateTime now = DateTime.Now;
            TimeSpan offset = now - now.ToUniversalTime();
            try
            {
                return File.GetLastWriteTimeUtc(path) + offset;
            }
            catch (Exception e)
            {
                return File.GetCreationTime(path);
            }
        }
    }
}