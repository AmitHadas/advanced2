using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        public List<string> ThumbPathList { get; set; }

        public PhotosModel(string outputDir)
        {
            string outi = "..\\OutputDir";
            ThumbPathList = new List<string>();
            string thumbnailPath = outi + "\\Thumbnails";
            foreach (string photo in Directory.GetFiles(thumbnailPath, "*", SearchOption.AllDirectories))
            {
                ThumbPathList.Add(photo);
            }
        }

    }
}