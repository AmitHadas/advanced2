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
        public List<ImageModel> ThumbPathList { get; set; }

        public PhotosModel(string outputDir)
        {
            ThumbPathList = new List<ImageModel>();
            string thumbnailPath = outputDir + "\\Thumbnails";
            string[] directoryFilesThumb = Directory.GetFiles(thumbnailPath, "*", SearchOption.AllDirectories);
            string[] directoryFilesOrigin = Directory.GetFiles(outputDir, "*", SearchOption.AllDirectories);
            foreach (string photo in directoryFilesOrigin)
            {

                if (!photo.Contains("Thumbnails"))
                {
                    var tokens = Regex.Split(photo, "OutputDir");
                    string originPathPhoto = "..\\..\\OutputDir" + tokens[1];
                    // thumbnail photo path
                    foreach (string thumbPhoto in directoryFilesThumb)
                    {
                        if (Path.GetFileName(thumbPhoto) == Path.GetFileName(photo)) {
                            var tokens1 = Regex.Split(thumbPhoto, "OutputDir");
                            string thumbPathPhoto = "..\\..\\OutputDir" + tokens1[1];
                            ThumbPathList.Add(new ImageModel(photo, thumbPhoto,originPathPhoto, thumbPathPhoto));
                        }

                    }
                }
            }
        }

    }
}