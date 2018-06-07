using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace ImageServiceWeb.Models
{
    public class ImageModel
    {
        public string RelativeThumbnailImagePath { get; set; }
        public string RelativeOriginalImagePath { get; set; }
        public string AbsoluteThumbnailImagePath { get; set; }
        public string AbsoluteOriginalImagePath { get; set; }
        public string ImageName { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }

        public ImageModel(string absolteOriginalPath, string absoluteThumbPath, string relativeOriginalPath, string relativeThumbPath)
        {
            this.AbsoluteOriginalImagePath = absolteOriginalPath;
            this.AbsoluteThumbnailImagePath = absoluteThumbPath;
            this.RelativeThumbnailImagePath = relativeThumbPath;
            this.RelativeOriginalImagePath = relativeOriginalPath;
            this.ImageName = Path.GetFileName(relativeThumbPath);
            DateTime dateTime = getImageDate(absolteOriginalPath);
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