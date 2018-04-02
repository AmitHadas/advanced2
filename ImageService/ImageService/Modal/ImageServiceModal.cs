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

        public string AddFile(string path, out bool result)
        {

            // if outputDir doesn't exists:
            if (!Directory.Exists(m_OutputFolder))
            {
                System.IO.Directory.CreateDirectory(m_OutputFolder);
                this.m_logging.Log("The output folder " + this.m_OutputFolder + " was created", Logging.Modal.MessageTypeEnum.INFO);
                System.IO.Directory.CreateDirectory(m_OutputFolder + "\\Thumbnails");
                this.m_logging.Log("The thumbnail folder was created", Logging.Modal.MessageTypeEnum.INFO);

            }

            Image myImage = Image.FromFile(path);
            PropertyItem propItem = null;
            string info = "";
            try
            {
                propItem = myImage.GetPropertyItem(20624);
                myImage.Dispose();
                //Convert date taken metadata to a DateTime object
                DateTime dateTime = File.GetCreationTime(path);

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
                    Directory.CreateDirectory(yearPath);
                    Directory.CreateDirectory(monthPath);
                    Directory.CreateDirectory(thumbYearPath);
                    Directory.CreateDirectory(thumbMonthPath);

                }
                else if (!Directory.Exists(monthPath))
                {
                    Directory.CreateDirectory(monthPath);
                    Directory.CreateDirectory(thumbMonthPath);
                }
               
                System.IO.File.Copy(path, monthPath + "\\" + imageName);
                this.m_logging.Log("The Image " + imageName + " was copied succesfully" , Logging.Modal.MessageTypeEnum.INFO);
                Image thumb = Image.FromFile(path);
                thumb = (Image)(new Bitmap(thumb, new Size(this.m_thumbnailSize, this.m_thumbnailSize)));
                thumb.Save(thumbMonthPath + "\\" + imageName);
                this.m_logging.Log("The thumbnail image " + imageName + " was created succesfully", Logging.Modal.MessageTypeEnum.INFO);
            }
            catch (Exception e)
            {
                info = e.Data.ToString();
                result = false;
            };

            return info;
        }
        
        /// ////////////להשתמש בזה
    
        public bool CreateFolder(string path)
        {
            System.IO.Directory.CreateDirectory(path);
            return true;
        }

        public bool MoveFile(string fileName, string srcPath, string dstPath)
        {
            System.IO.File.Move(srcPath, dstPath);
            return true;
        }
    }

}