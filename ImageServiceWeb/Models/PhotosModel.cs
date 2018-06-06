using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class PhotosModel
    {
        public List<Image> ThumbPathList { get; set; }

        public PhotosModel(string outputDir)
        {
            ThumbPathList = new List<Image>();
            string thumbnailPath = outputDir + "\\Thumbnails";
            string[] directoryFiles = Directory.GetFiles(thumbnailPath, "*", SearchOption.AllDirectories);
            foreach (string photo in directoryFiles)
            {
                var tokens = Regex.Split(photo, "OutputDir");
                string pathPhoto = "..\\..\\OutputDir" + tokens[1]; 
                ThumbPathList.Add(new Image(pathPhoto));
            }
        }

    }
}