using ImageService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;
using ImageService.Logging;

namespace ImageService.Modal
{
    public class ImageServiceModal : IImageServiceModal
    {

        private string m_OutputFolder;            // The Output Folder
        private int m_thumbnailSize;              // The Size Of The Thumbnail Size
        private ILoggingService m_logging;
        public ImageServiceModal(ILoggingService logging)
        {
            this.m_logging = logging;
            this.m_OutputFolder = ConfigurationManager.AppSettings.Get("OutputDir");
            this.m_thumbnailSize = int.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
        }
        public bool ThumbnailCallback()
        {
            return true;
        }

        //The function returns the creation date of the image.
        public static DateTime getImageDate(string path)
        {
            DateTime now = DateTime.Now;
            TimeSpan offset = now - now.ToUniversalTime();
            try
            {
                return File.GetLastWriteTimeUtc(path) + offset;
            } catch (Exception e)
            {
                return File.GetCreationTime(path);
            }
        }


        //The function creates direcories according to the date of the image,
        //creates thumbnail file and moves the image to the output direcory.
        public string AddFile(string path, out bool result)
        {

            // if outputDir doesn't exists:
            if (!Directory.Exists(m_OutputFolder))
            {
                this.CreateFolder(m_OutputFolder);
                this.m_logging.Log("The output folder " + this.m_OutputFolder + " was created", Logging.Modal.MessageTypeEnum.INFO);
                this.CreateFolder(m_OutputFolder + "\\Thumbnails");
                this.m_logging.Log("The thumbnail folder was created", Logging.Modal.MessageTypeEnum.INFO);

            }
            System.Threading.Thread.Sleep(100);
            Image myImage = Image.FromFile(path);
            PropertyItem propItem = null;
            string info = "";
            try
            {
                propItem = myImage.GetPropertyItem(20624);
                myImage.Dispose();
                //Convert date taken metadata to a DateTime object
                 DateTime dateTime = getImageDate(path);

                //create path of the image
                string year = dateTime.Year.ToString();
                string yearPath = this.m_OutputFolder + "\\" + year;
                string thumbMonthPath = this.m_OutputFolder + "\\Thumbnails\\" + year + "\\" + dateTime.Month;
                string thumbYearPath = this.m_OutputFolder + "\\Thumbnails\\" + year;
                string monthPath = this.m_OutputFolder + "\\" + year + "\\" + dateTime.Month;
                info = monthPath;
                result = true;
                string imageName = Path.GetFileName(path);

                if (!Directory.Exists(yearPath))
                {
                    this.CreateFolder(yearPath);
                    this.CreateFolder(monthPath);
                    this.CreateFolder(thumbYearPath);
                    this.CreateFolder(thumbMonthPath);

                }
                else if (!Directory.Exists(monthPath))
                {
                    this.CreateFolder(monthPath);
                    this.CreateFolder(thumbMonthPath);
                }
                // sleep for 100 milisecondes.
                System.Threading.Thread.Sleep(100);
                string dstPath = monthPath + "\\" + imageName;
                this.MoveFile(ref imageName, path, ref dstPath);
                this.m_logging.Log("The Image " + imageName + " was moved succesfully" , Logging.Modal.MessageTypeEnum.INFO);
                Image thumb = Image.FromFile(dstPath);
                thumb = (Image)(new Bitmap(thumb, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
                thumb.Save(thumbMonthPath + "\\" + imageName);
                myImage.Dispose();
                this.m_logging.Log("The thumbnail image " + imageName + " was created succesfully", Logging.Modal.MessageTypeEnum.INFO);
                try
                {
                    System.IO.File.Delete(path);
                } catch (Exception e)
                {
                   
                }
            }
            catch (Exception e)
            {
                info = e.Data.ToString();
                result = false;
            };

            return info;
        }
  
    
        //The function creates folder in the path.
        public bool CreateFolder(string path)
        {
            System.IO.Directory.CreateDirectory(path);
            DirectoryInfo dir = new DirectoryInfo(path);
            dir.Attributes |= FileAttributes.Hidden;
            return true;
        }

        //The funtion move the file from srcPath to dstPath
        public bool MoveFile(ref string fileName, string srcPath, ref string dstPath)
        {

            int count = 1;

            string fileNameOnly = Path.GetFileNameWithoutExtension(dstPath);
            string extension = Path.GetExtension(dstPath);
            string path = Path.GetDirectoryName(dstPath);
            string newFullPath = dstPath;

            while (File.Exists(newFullPath))
            {
                string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                newFullPath = Path.Combine(path, tempFileName + extension);
                fileName = tempFileName + extension;
            }
            System.Threading.Thread.Sleep(100);
            System.IO.File.Move(srcPath, newFullPath);
            dstPath = newFullPath;
            return true;
        }
    }

}